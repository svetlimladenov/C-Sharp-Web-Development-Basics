namespace ChushkaWebApp.ViewModels.Orders
{
    public class BaseOrderViewModel
    {
        public int Id { get; set; }

        public string Customer { get; set; }

        public string Product { get; set; }

        public string OrderedOn { get; set; }
    }
}
