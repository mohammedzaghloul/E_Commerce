using E_Commerce.Models.UserFile;

namespace E_Commerce.DataAccessData.Configurations.UserConfigurations
{
    public class UserAddressesConfiguration : IEntityTypeConfiguration<UserAddresses>
    {
        public void Configure(EntityTypeBuilder<UserAddresses> builder)
        {
            builder.HasOne(ads => ads.User)
                .WithMany(u => u.UserAddresses)
                .HasForeignKey(ads => ads.UserId);

            builder.HasOne(ads => ads.UserAddress)
           .WithMany(ad => ad.UserAddresses)
           .HasForeignKey(ads => ads.AddressId);

            builder.HasKey(ads => new { ads.UserId, ads.AddressId });
        }
    }
}
