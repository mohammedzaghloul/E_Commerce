using E_Commerce.Models.UserFile;

namespace E_Commerce.DataAccessData.Configurations.UserConfigurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(a => a.Region)
                .IsRequired();

            builder.Property(a => a.PostalCode)
                .IsRequired();

            builder.Property(a => a.PostalCode)
                .IsRequired();
            

            builder.HasOne(a => a.Governorate)
                .WithMany(g => g.UserAddresses)
                .HasForeignKey(a => a.GovernorateID);
        }
    }
}
