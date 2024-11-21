using E_Commerce.Models.OrderFile;

namespace E_Commerce.DataAccessData.Configurations.OrderFileConfigurations
{
    public class TaxConfiguration : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder.Property(p => p.TaxRate).IsRequired();
        }
    }
}
