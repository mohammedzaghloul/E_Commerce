namespace E_Commerce.Models.Product
{
    public class Promotion
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public int DiscountRate { get; set; }

        public virtual ICollection<CategoryPromotion>? PromotionCategories { get; set; } = new HashSet<CategoryPromotion>();

    }
}
