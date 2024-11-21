using E_Commerce.Models.OrderFile;

namespace E_Commerce.DataAccessData.Configurations.OrderFileConfigurations
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.HasKey(ol => ol.Id);
     
            builder.ToTable("OrderLines");

            builder.HasOne(ol => ol.Product)
                   .WithMany(p => p.OrderLines)
                   .HasForeignKey(ol => ol.ProductItemId);

            builder.HasOne(ol => ol.Order)
                   .WithMany(o => o.OrderLines)
                   .HasForeignKey(ol => ol.OrderId);


            builder.Property(ol => ol.Quantity)
                .IsRequired();

            builder.Property(ol => ol.Price)
                .IsRequired();

        }
    }
}
