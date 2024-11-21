using E_Commerce.Models.Product;

namespace E_Commerce.DataAccessData.Configurations.ProductFileConfigurations
{
    public class CategoryPromotionConfiguration : IEntityTypeConfiguration<CategoryPromotion>
    {
        public void Configure(EntityTypeBuilder<CategoryPromotion> builder)
        {
            builder.HasOne(cp => cp.Promotion)
                .WithMany(promotion => promotion.PromotionCategories)
                .HasForeignKey(cp => cp.PromotionId);

            builder.HasOne(cp => cp.Category)
                .WithMany(pc => pc.PromotionCategories)
                .HasForeignKey(cp => cp.CategoryId);

            /// composite primary key
            builder.HasKey(p => new {p.PromotionId, p.CategoryId});
        }
    }
}
