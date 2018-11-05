using System.Linq;
using Microsoft.EntityFrameworkCore;
using PandaWebApp.ViewModels.Receipts;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace PandaWebApp.Controllers
{
    public class ReceiptsController : BaseController
    {
        [Authorize]
        public IHttpResponse Index()
        {
            var viewModel = new ReceiptIndexViewModel();
            viewModel.Receipts = this.Db.Receipts.Include(r => r.Recipient)
                .Where(r => r.Recipient.Username == this.User.Username)
                .Select(r => new BaseReceiptIndexViewModel()
                {
                    Recipient = r.Recipient.Username,
                    Fee = r.Fee,
                    Id = r.Id,
                    IssuedOn = r.IssuedOn.ToString("d"),
                }).ToList();

            return this.View(viewModel);
        }
        
        [Authorize]
        public IHttpResponse Details(int id)
        {
            //TODO SECURE
            var receipt =
                this.Db.Receipts.Include(r => r.Recipient).FirstOrDefault(r => r.Id == id && r.Recipient.Username == this.User.Username);
            
            if (receipt == null)
            {
                return BadRequestError("Invalid receipt.");
            }

            var package = this.Db.Packages.FirstOrDefault(x => x.Id == receipt.PackageId);
            var viewModel = new ReceiptDetailsViewModel()
            {
                DeliveryAddress = package.ShippingAddress,
                Description = package.Description,
                Recipient = receipt.Recipient.Username,
                Id = receipt.Id,
                Weight = package.Weight,
                IssuedOn = receipt.IssuedOn.ToString("d"),
                Fee = receipt.Fee
            };
            return this.View(viewModel);
        }
    }
}
