using System;
using CakesWebApp.Models;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;

namespace CakesWebApp.Controllers
{
    public class CakeController : BaseController
    {
        public IHttpResponse CakePage(IHttpRequest request)
        {
            return this.View("add");
        }

        public IHttpResponse AddCake(IHttpRequest request)
        {
            var cakeName = request.FormData["name"].ToString().Trim();
            var price = decimal.Parse(request.FormData["price"].ToString().Trim());
            var pictureUrl = request.FormData["pictureUrl"].ToString().Trim();

            if (price <= 0)
            {
                return this.BadRequestError("Price must be a positive number.");
            }

            this.Db.Products.Add(new Product()
            {
                Name = cakeName,
                ImageUrl = pictureUrl,
                Price = price
            });

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return new RedirectResult("/");
        }
    }
}
