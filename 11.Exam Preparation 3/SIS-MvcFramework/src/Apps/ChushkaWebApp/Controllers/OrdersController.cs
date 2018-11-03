using System.Linq;
using ChushkaWebApp.ViewModels.Orders;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace ChushkaWebApp.Controllers
{
    public class OrdersController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse All()
        {
            var viewModel = new AllOrdersViewModel();

            var allOrders = this.Db.Orders.Select(o => new BaseOrderViewModel()
            {
                Customer = o.User.Username,
                Product = o.Product.Name,
                Id = o.Id,
                OrderedOn = o.OrderedOn.ToString("d"),
            }).ToArray();

            viewModel.Orders = allOrders;

            return this.View(viewModel);
        }
    }
}
