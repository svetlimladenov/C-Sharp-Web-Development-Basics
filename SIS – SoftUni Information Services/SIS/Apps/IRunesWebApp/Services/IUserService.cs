using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;

namespace IRunesWebApp.Services
{
    public interface IUserService
    {
        IHttpResponse Register(IHttpRequest request);
        IHttpResponse Login(IHttpRequest request);

    }
}
