using System.Linq;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses;

namespace TorshiaWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.Username == null)
            {
                return this.View("Home/IndexLoggedOut");
            }

            var viewModel = new IndexViewModel();

            viewModel.Tasks = this.Db.Tasks.Include(t => t.TaskSectors).ThenInclude(ts => ts.Sector).Where(t => t.IsReported == false).Select(t => new BaseTaskViewModel()
            {
                Id = t.Id,
                Title = t.Title,
                Level = t.TaskSectors.Select(ts => ts.Sector).Count(),
            }).ToArray();


            return this.View(viewModel);
        }

        
    }
}
