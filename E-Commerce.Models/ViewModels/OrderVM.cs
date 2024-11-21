using E_Commerce.Models.OrderFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Models.ViewModels
{
    public class OrderVM
    {
        public Order  OrderHeader { get; set; }
        public IEnumerable<OrderLine> OrderDetail { get; set; }
    }
}
