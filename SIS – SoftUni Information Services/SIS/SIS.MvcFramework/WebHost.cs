﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            var dependencyContainer = new ServiceCollection();
            application.ConfigureServices(dependencyContainer);

            var serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(serverRoutingTable, application, dependencyContainer);

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

                    var path = httpAttribute.Path;
                    if (!path.StartsWith("/"))
                    {
                        path = "/" + path;
                    }
                    routingTable.Add(httpAttribute.Method, path,
                        (request) => ExecuteAction(controller, methodInfo, request, serviceCollection));

                    Console.WriteLine($"Route registered : {controller.Name}.{methodInfo.Name} => {httpAttribute.Method} => {httpAttribute.Path}");
                }

            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            var controllerInstance = serviceCollection.CreateInstance(controllerType) as Controller;
            if (controllerInstance == null)
            {
                return new TextResult("Controller not found", HttpResponseStatusCode.InternalServerError);
            }

            controllerInstance.Request = request;
            controllerInstance.ViewEngine = new ViewEngine.ViewEngine();
            controllerInstance.UserCookieService = serviceCollection.CreateInstance<IUserCookieService>();
            


            var actionParameterObjects = GetActionParameterObjects(methodInfo, request, serviceCollection);

            var httpResponse = methodInfo.Invoke(controllerInstance, actionParameterObjects.ToArray()) as IHttpResponse;

            return httpResponse;
        }

        private static List<object> GetActionParameterObjects(MethodInfo methodInfo, IHttpRequest request,
            IServiceCollection serviceCollection)
        {
            var actionParameters = methodInfo.GetParameters();
            var actionParameterObjects = new List<object>();
            foreach (var actionParameter in actionParameters)
            {
                //TODO : Improce this check
                if (actionParameter.ParameterType.IsValueType || Type.GetTypeCode(actionParameter.ParameterType) == TypeCode.String)
                {
                    var stringValue = GetRequestData(request, actionParameter.Name);
                    actionParameterObjects.Add(ObjectMapper.TryParse(stringValue, actionParameter.ParameterType));
                }
                else
                {

                    var instance = serviceCollection.CreateInstance(actionParameter.ParameterType);
                    var properties = actionParameter.ParameterType.GetProperties();
                    foreach (var propertyInfo in properties)
                    {
                        //TODO: Support IEnumerable
                        var key = propertyInfo.Name;
                        var stringValue = GetRequestData(request, key);
                        var value = ObjectMapper.TryParse(stringValue, propertyInfo.PropertyType);


                        propertyInfo.SetMethod.Invoke(instance, new object[] { value });
                    }

                    actionParameterObjects.Add(instance);
                }

            }

            return actionParameterObjects;
        }

        private static string GetRequestData(IHttpRequest request, string key)
        {
            key = key.ToLower();
            string stringValue = null;
            if (request.FormData.Any(x => x.Key.ToLower() == key))
            {
                stringValue = request.FormData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();
            }
            else if (request.QueryData.Any(x => x.Key.ToLower() == key))
            {
                stringValue = request.QueryData.FirstOrDefault(x => x.Key.ToLower() == key).Value.ToString()
                    .UrlDecode();
            }

            return stringValue;
        }

       
    }
}
