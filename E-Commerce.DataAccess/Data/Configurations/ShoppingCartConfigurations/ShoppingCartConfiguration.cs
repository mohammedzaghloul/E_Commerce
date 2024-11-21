using E_Commerce.Models.ShoppingCartFile;

namespace E_Commerce.DataAccessData.Configurations.ShoppingCartConfigurations
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.Property(si => si.Quantity)
                .IsRequired();

            builder.HasOne(si => si.ProductItem)
                .WithMany(pi => pi.ShoppingCart)
                .HasForeignKey(si => si.ProductItemId);


        }
    }
}
