using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;

namespace IRunesWebApp.Controller
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            return this.View("Index");
        }
    }
}
