using System.Collections.Generic;

namespace ChushkaWebApp.Models
{
    public class Product
    {
        public Product()
        {
            this.Orders = new HashSet<Order>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public ProductType Type { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
