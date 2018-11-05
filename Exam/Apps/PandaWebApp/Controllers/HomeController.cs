using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PandaWebApp.Models;
using PandaWebApp.ViewModels.Home;

namespace PandaWebApp.Controllers
{
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (!this.User.IsLoggedIn)
            {
                //TODO: FIX NAVBAR
                return this.View("Home/IndexLoggedOut");
            }

            var user = this.Db.Users.Include(u => u.Packages).FirstOrDefault(u => u.Username == this.User.Username);
            var pending = user.Packages.Where(p => p.Status == PackageStatus.Pending).Select(p => new BasePackageViewModel()
            {
                Description = p.Description,
                Id = p.Id,
            });

            var shipped = user.Packages.Where(p => p.Status == PackageStatus.Shipped).Select(p => new BasePackageViewModel()
            {
                Description = p.Description,
                Id = p.Id,
            });

            var delivered = user.Packages.Where(p => p.Status == PackageStatus.Delivered).Select(p => new BasePackageViewModel()
            {
                Description = p.Description,
                Id = p.Id,
            });


            var viewModel = new IndexViewModel();
            viewModel.Pending = pending.ToList();
            viewModel.Shipped = shipped.ToList();
            viewModel.Delivered = delivered.ToList();
            return this.View(viewModel);
        }
    }
}
