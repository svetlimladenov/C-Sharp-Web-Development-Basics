﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CakesWebApp.Extensions;
using CakesWebApp.Models;
using CakesWebApp.ViewModels.Cakes;
using SIS.Http.Requests;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework;
using SIS.WebServer.Results;

namespace CakesWebApp.Controllers
{
    public class CakesController : BaseController
    {
        [HttpGet("/cakes/add")]
        public IHttpResponse AddCakes()
        {
            return this.View("AddCakes");
        }

        [HttpPost("/cakes/add")]
        public IHttpResponse DoAddCakes(DoAddCakesModel model)
        {
            // TODO: Validation

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price.ToDecimal(),
                ImageUrl = model.Picture
            };
            this.Db.Products.Add(product);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return this.ServerError(e.Message);
            }

            // Redirect
            return this.Redirect("/");
        }

        //cakes/view/?id=1
        [HttpGet("/cakes/view")]
        public IHttpResponse ById()
        {
            var id = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.Db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return this.BadRequestError("Cake not found.");
            }

            //TODO: to view model
            var viewBag = new Dictionary<string, string>
            {
                {"Name", product.Name},
                {"Price", product.Price.ToString(CultureInfo.InvariantCulture)},
                {"ImageUrl", product.ImageUrl}
            };
            return this.View("CakeById", viewBag);
        }
    }
}
