using System.Linq;
using ChushkaWebApp.ViewModels.Home;
using SIS.HTTP.Responses;

namespace ChushkaWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (!this.User.IsLoggedIn)
            {
                return this.View("Home/IndexLoggedOut");
            }

            var viewModel = new IndexViewModel();
            viewModel.Products = this.Db.Products.Where(p => !p.IsDeleted).Select(p => new BaseProductViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            }).ToList();

            return this.View(viewModel);
        }

        
    }
}
