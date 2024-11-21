
using E_Commerce.Models.OrderFile;

namespace E_Commerce.DataAccessData.Configurations.OrderFileConfigurations
{
    public class OrderImportancyConfiguration : IEntityTypeConfiguration<OrderImportancy>
    {
        public void Configure(EntityTypeBuilder<OrderImportancy> builder)
        {
            builder.HasKey(o => o.Id);

            builder.ToTable("OrderImportancies");

            builder.Property(o => o.Name)
                  .IsRequired() 
                  .HasMaxLength(100);

            builder.Property(o => o.Price)
                  .IsRequired();

        }
    }
}
