using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitslnStock { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public void TryValidate()
        {
            if (ProductName == null)
            {
                throw new ArgumentNullException("ProductName");
            }
            if (Weight == null)
            {
                throw new ArgumentException("Weight");
            }
        }
    }
}
