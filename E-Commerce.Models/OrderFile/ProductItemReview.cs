using E_Commerce.Models.UserFile;

namespace E_Commerce.Models.OrderFile
{
    public class ProductItemReview
    {
        public int Id { get; set; }
        public int OrderLineId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }

        public virtual OrderLine? OrderLine { get; set; }
        public virtual User? User { get; set; }

    }
}
