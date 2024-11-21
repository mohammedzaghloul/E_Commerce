using E_Commerce.Models.Product;
using E_Commerce.Models.UserFile;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models.ShoppingCartFile
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; }

        [Range(1, 100, ErrorMessage ="Please enter a value between 1 and 100")]
        public int Quantity { get; set; }
        public string ApplicaitonUserId { get; set; }

        [ForeignKey("ProductItemId")]
        public virtual ProductItem? ProductItem { get; set; }

        [ForeignKey("ApplicaitonUserId")]
        [ValidateNever]
        public virtual User User { get; set; }

        [NotMapped]
        public double Price {  get; set; }
    }
}
