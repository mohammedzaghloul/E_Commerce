using E_Commerce.Models.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Models.ViewModels
{
    public class ProductItemWithProductSelectListVM
    {
        public ProductItem ProductItem { get; set; }
        public IEnumerable<SelectListItem>? ProductList {  get; set; } 
    }
}
