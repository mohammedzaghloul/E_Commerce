using E_Commerce.Models.Product;

namespace E_Commerce.Models.OrderFile
{
    //Order Details
    public class OrderLine
    {
        public int Id { get; set; }
        
        public int ProductItemId { get; set; }

        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
        public virtual ProductItem? Product { get; set; }
        public virtual Order? Order { get; set; }

        public virtual ICollection<ProductItemReview>? ProductItemReviews { get; set; } = new HashSet<ProductItemReview>();
    }
}
