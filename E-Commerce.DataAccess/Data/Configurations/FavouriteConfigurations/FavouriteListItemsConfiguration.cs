using E_Commerce.Models.Favourite;

namespace E_Commerce.DataAccessData.Configurations.FavouriteConfiguration
{
    public class FavouriteListItemsConfiguration : IEntityTypeConfiguration<FavouriteListItems>
    {
        public void Configure(EntityTypeBuilder<FavouriteListItems> builder)
        {
            builder.HasKey(f => new
            {
                f.ProductItemId,
                f.UserId
            }); 

            builder.HasOne(p => p.ProductItem)
                .WithMany(f => f.FavouriteListItems)
                .HasForeignKey(f => f.ProductItemId);

            builder.HasOne(p => p.User)
               .WithMany(f => f.FavouriteListItems)
               .HasForeignKey(f => f.UserId);

          
        }
    }
}
