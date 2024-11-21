using E_Commerce.Models.UserFile;

namespace E_Commerce.DataAccessData.Configurations.UserConfigurations
{
    public class GovernorateConfiguration : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(g => g.OrderPrice)
                .IsRequired();
        }
    }
}
