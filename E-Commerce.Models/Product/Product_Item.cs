using E_Commerce.Models.Favourite;
using E_Commerce.Models.OrderFile;
using E_Commerce.Models.ShoppingCartFile;

namespace E_Commerce.Models.Product
{
    public class ProductItem
    {
        public int Id { get; set; }
        public string SKU { get; set; }

        public int QuantityInStock { get; set; }

        public string? ImageUrl { get; set; }
        public double Price { get; set; }

        //[ForeignKey("Products")]
        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual ICollection<FavouriteListItems>? FavouriteListItems { get; set; } = new HashSet<FavouriteListItems>();
        public virtual ICollection<ShoppingCart>? ShoppingCart { get; set; } = new HashSet<ShoppingCart>();
        public virtual ICollection<OrderLine>? OrderLines { get; set; } = new HashSet<OrderLine>();


    }
}
