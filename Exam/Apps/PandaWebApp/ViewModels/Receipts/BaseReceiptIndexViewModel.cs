namespace PandaWebApp.Controllers
{
    public class BaseReceiptIndexViewModel
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public string IssuedOn { get; set; }

        public string Recipient { get; set; }
    }
}