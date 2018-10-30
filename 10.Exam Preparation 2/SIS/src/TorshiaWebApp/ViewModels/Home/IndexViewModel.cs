using System.Collections.Generic;

namespace TorshiaWebApp.Controllers
{
    public class IndexViewModel
    {
        public ICollection<BaseTaskViewModel> Tasks { get; set; }
    }
}