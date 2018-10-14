using System;
using System.Linq;
using System.Reflection;
using CakesWebApp.Services;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework.Services;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            var dependencyContainer = new ServiceCollection();
            application.ConfigureServices(dependencyContainer);

            var serverRoutingTable = new ServerRoutingTable(); 
            AutoRegisterRoutes(serverRoutingTable,application,dependencyContainer);
                  
            application.Configure();

            var server = new Server(80, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable routingTable, IMvcApplication application, IServiceCollection serviceCollection)
        {
            var controllers = application.GetType().Assembly.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Controller)));

            foreach (var controller in controllers)
            {
                var getMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(method => method.CustomAttributes.Any(ca => ca.AttributeType.IsSubclassOf(typeof(HttpAttribute))));

                foreach (var methodInfo in getMethods)
                {
                    var httpAttribute = (HttpAttribute)methodInfo.GetCustomAttributes(true).FirstOrDefault(ca =>
                        ca.GetType().IsSubclassOf(typeof(HttpAttribute)));
                    if (httpAttribute == null)
                    {
                        continue;
                    }
                    routingTable.Add(httpAttribute.Method,httpAttribute.Path, 
                        (request) => ExecuteAction(controller, methodInfo, request,serviceCollection));

                    Console.WriteLine($"Route registered : {controller.Name}.{methodInfo.Name} => {httpAttribute.Method} => {httpAttribute.Path}");
                }
                
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            var controllerInstance = serviceCollection.CreateInstance(controllerType) as Controller;
            if (controllerInstance == null)
            {
                return new TextResult("Controller not found",HttpResponseStatusCode.InternalServerError);
            }

            controllerInstance.Request = request;
            controllerInstance.UserCookieService = serviceCollection.CreateInstance<IUserCookieService>();

            var httpResponse =  methodInfo.Invoke(controllerInstance, new object[] { }) as IHttpResponse;
            //1. Create instance of controllerName
            //2. Set request
            //3. Invoke actionName
            //4. Return action result
            return httpResponse;
        } 
    }
}
