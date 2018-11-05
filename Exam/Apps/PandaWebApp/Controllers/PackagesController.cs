using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PandaWebApp.Models;
using PandaWebApp.ViewModels.Packages;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace PandaWebApp.Controllers
{
    public class PackagesController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            var viewModel = new CreatePackageViewModel();
            viewModel.Recipients = this.Db.Users.Select(u => new BaseUserViewModel()
            {
                Username = u.Username
            }).ToList();

            return this.View(viewModel);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(CreatePackageInputModel model)
        {
            var recipient = this.Db.Users.Include(u => u.Packages).FirstOrDefault(u => u.Username == model.Recipient);
            var package = new Package()
            {
                RecipientId = recipient.Id,
                Description = model.Description,
                Status = PackageStatus.Pending,
                Weight = model.Weight,
                ShippingAddress = model.ShippingAddress,
            };

            recipient.Packages.Add(package);
            this.Db.Add(package);
            this.Db.SaveChanges();
            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Pending()
        {
            var viewModel = new PendingShippedDeliveredViewModel();
            viewModel.Pending = this.Db.Packages.Where(p => p.Status == PackageStatus.Pending).Include(p => p.Recipient).Select(p =>
                new BasePendingPageViewModel()
                {
                    Description = p.Description,
                    Recipient = p.Recipient.Username,
                    Id = p.Id,
                    ShippingAddress = p.ShippingAddress,
                    Weight = p.Weight
                }).ToList();
            return this.View(viewModel);

        }

        public IHttpResponse Ship(int id)
        {
            var package = this.Db.Packages.FirstOrDefault(p => p.Id == id);
            if (package == null)
            {
                return this.BadRequestError("Invalid package");
            }
            package.Status = PackageStatus.Shipped;
            var random = new Random();
            var estimateDelivery = random.Next(20, 40);
            package.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(estimateDelivery);
            this.Db.SaveChanges();
            return this.Redirect("/Packages/Pending");
        }

        [Authorize("Admin")]
        public IHttpResponse Shipped()
        {
            var viewModel = new PendingShippedDeliveredViewModel();
            viewModel.Shipped = this.Db.Packages.Where(p => p.Status == PackageStatus.Shipped).Include(p => p.Recipient).Select(p =>
                new BasePendingPageViewModel()
                {
                    Description = p.Description,
                    Recipient = p.Recipient.Username,
                    Id = p.Id,
                    ShippingAddress = p.ShippingAddress,
                    Weight = p.Weight
                }).ToList();
            return this.View(viewModel);

        }

        [Authorize("Admin")]
        public IHttpResponse Deliver(int id)
        {
            var package = this.Db.Packages.FirstOrDefault(p => p.Id == id);
            if (package == null)
            {
                return this.BadRequestError("Invalid package");
            }
            package.Status = PackageStatus.Delivered;
            this.Db.SaveChanges();
            return this.Redirect("/Packages/Shipped");
        }

        [Authorize("Admin")]
        public IHttpResponse Delivered()
        {
            var viewModel = new PendingShippedDeliveredViewModel();
            viewModel.Delivered = this.Db.Packages.Where(p => p.Status == PackageStatus.Delivered || p.Status == PackageStatus.Acquired).Include(p => p.Recipient).Select(p =>
                new BasePendingPageViewModel()
                {
                    Description = p.Description,
                    Recipient = p.Recipient.Username,
                    Id = p.Id,
                    ShippingAddress = p.ShippingAddress,
                    Weight = p.Weight
                }).ToList();
            return this.View(viewModel);
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var viewModel = this.Db.Packages.Where(p => p.Id == id).Include(p => p.Recipient)
                .Select(p => new PackageDetailsViewModel()
                {
                    Status = p.Status.ToString(),
                    Address = p.ShippingAddress,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate.ToString("d"),
                    Recipient = p.Recipient.Username,
                    Description = p.Description,
                    Weight = p.Weight
                }).FirstOrDefault();
            if (this.User.Username != viewModel.Recipient)
            {
                return BadRequestError("Invalid package.");
            }
            return this.View(viewModel);
        }


        [Authorize]
        public IHttpResponse Acquire(int id)
        {
            var package = this.Db.Packages.Include(p => p.Recipient).FirstOrDefault(p => p.Id == id);
            if (package == null)
            {
                return this.BadRequestError("Invalid package");
            }
            package.Status = PackageStatus.Acquired;
            var receipUser = this.Db.Users.FirstOrDefault(u => u.Username == package.Recipient.Username);
            var receipt = new Receipt()
            {
                RecipientId = package.Recipient.Id,
                IssuedOn = DateTime.UtcNow,
                PackageId = package.Id,
                Fee = (decimal)(package.Weight * 2.67),
            };
            receipUser.Receipts.Add(receipt);
            this.Db.Receipts.Add(receipt);
            this.Db.SaveChanges();
            return this.Redirect("/Receipts/Index");
        }
    }
}
