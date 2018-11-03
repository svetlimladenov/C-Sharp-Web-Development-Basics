using System;
using System.Linq;
using ChushkaWebApp.Models;
using ChushkaWebApp.ViewModels.Home;
using ChushkaWebApp.ViewModels.Products;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace ChushkaWebApp.Controllers
{
    public class ProductsController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var viewModel = this.Db.Products.Where(p => !p.IsDeleted).Select(p => new BaseProductViewModel()
            {
                Name = p.Name.Trim(),
                Description = p.Description.Trim(),
                Price = p.Price,
                Id = p.Id,
                Type = p.Type.ToString().Trim(),
            }).FirstOrDefault(p => p.Id == id);

            if (viewModel == null)
            {
                return BadRequestError("Invalid product.");
            }
            return this.View(viewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize("Admin")]
        public IHttpResponse Create(CreateProductInputViewModel model)
        {
            if (!Enum.TryParse(model.Type, out ProductType type))
            {
                return this.BadRequestErrorWithView("Invalid type.");
            }

            var product = new Product()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Type = type
            };

            this.Db.Products.Add(product);
            this.Db.SaveChanges();

            return this.Redirect("/Products/Details?id=" + product.Id);
        }

        [Authorize("Admin")]
        public IHttpResponse Edit(int id)
        {
            var product = this.Db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return BadRequestError("Invalid Product");
            }

            if (product.IsDeleted == true)
            {
                return BadRequestError("Invalid Product");
            }

            var viewModel = new CreateProductInputViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = product.Type.ToString(),
            };
            return this.View(viewModel);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Edit(CreateProductInputViewModel model)
        {
            var currentProduct = this.Db.Products.FirstOrDefault(p => p.Id == model.Id);
            if (currentProduct == null)
            {
                return this.BadRequestError("Invalid product.");
            }
            if (!Enum.TryParse(model.Type, out ProductType type))
            {
                return this.BadRequestErrorWithView("Invalid type.");
            }
            currentProduct.Description = model.Description;
            currentProduct.Name = model.Name;
            currentProduct.Price = model.Price;
            currentProduct.Type = type;

            this.Db.SaveChanges();
            
            return this.Redirect("/Products/Details?id=" + model.Id);
        }


        [Authorize("Admin")]
        public IHttpResponse Delete(int id)
        {
            var product = this.Db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return BadRequestError("Invalid Product");
            }

            if (product.IsDeleted == true)
            {
                return BadRequestError("Invalid Product");
            }


            var viewModel = new CreateProductInputViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = product.Type.ToString(),
            };
            return this.View(viewModel);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Delete(int id, string username)
        {
            var product = this.Db.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return BadRequestError("Product does not exist.");
            }

            if (product.IsDeleted)
            {
                return BadRequestError("Product already deleted");
            }

            product.IsDeleted = true;

            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Order(int id)
        {
            var product = this.Db.Products.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (product == null)
            {
                return BadRequestError("Invalid product.");
            }

            // ReSharper disable once PossibleNullReferenceException
            var userId = this.Db.Users.FirstOrDefault(u => u.Username == this.User.Username).Id;
            var order = new Order()
            {
                OrderedOn = DateTime.UtcNow,
                ProductId = product.Id,
                UserId = userId,                
            };

            this.Db.Orders.Add(order);
            this.Db.SaveChanges();
            return this.Redirect("/");
        }
    }
}
