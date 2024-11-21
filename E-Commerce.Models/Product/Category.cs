using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models.Product
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }

        [Display (Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; } = new HashSet<Category>();
        public virtual ICollection<CategoryPromotion>? PromotionCategories { get; set; } = new HashSet<CategoryPromotion>();
        public virtual ICollection<Product>? Products { get; set; } = new HashSet<Product>();

    }
}
