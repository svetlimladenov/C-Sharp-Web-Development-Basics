using System.Collections.Generic;
using PandaWebApp.Controllers;

namespace PandaWebApp.ViewModels.Home
{
    public class IndexViewModel
    {
        public ICollection<BasePackageViewModel> Pending { get; set; }

        public ICollection<BasePackageViewModel> Shipped { get; set; }

        public ICollection<BasePackageViewModel> Delivered { get; set; }

    }
}