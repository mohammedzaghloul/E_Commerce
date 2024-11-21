using E_Commerce.Models.Product;

namespace E_Commerce.DataAccessData.Configurations.ProductFileConfigurations
{
    public class PromotionConfigurations : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.Property(P => P.Name)
                .HasMaxLength(200);

            builder.Property(P => P.Description)
                .HasMaxLength(500);

            builder.Property(P => P.StartDate).IsRequired();

            builder.Property(P => P.EndDate).IsRequired();

            builder.Property(P => P.DiscountRate).IsRequired(); /// discount rate is an int data type :)


        }
    }
}
