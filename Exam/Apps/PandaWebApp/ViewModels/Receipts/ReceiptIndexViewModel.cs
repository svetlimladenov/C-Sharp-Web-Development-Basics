using System.Collections.Generic;

namespace PandaWebApp.Controllers
{
    public class ReceiptIndexViewModel
    {
        public ICollection<BaseReceiptIndexViewModel> Receipts { get; set; }
    }
}