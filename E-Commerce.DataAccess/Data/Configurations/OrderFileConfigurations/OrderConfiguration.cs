using E_Commerce.Models.OrderFile;

namespace E_Commerce.DataAccessData.Configurations.OrderFileConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

                    builder.Property(o => o.OrderDate)
                   .IsRequired(); 

            builder.Property(o => o.TotalPrice)
                   .IsRequired() 
                   .HasColumnType("decimal(10,2)");


            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
                   

            builder.HasOne(o => o.UserPaymentMethod)
                   .WithMany(u => u.Orders) 
                   .HasForeignKey(o => o.PaymentMethodId);

            builder.HasOne(o => o.UserAddress)
                   .WithMany(a => a.Orders) 
                   .HasForeignKey(o => o.AddressId);

            builder.HasOne(o => o.OrderStatus)
                   .WithMany(s=>s.Orders) 
                   .HasForeignKey(o => o.OrderStatusId);

            builder.HasOne(o => o.OrderImportancy)
                   .WithMany(i => i.Orders) 
                   .HasForeignKey(o => o.ImportancyId);

            builder.HasOne(o => o.Tax)
                   .WithMany(t => t.Orders) 
                   .HasForeignKey(o => o.TaxId);

        }
    }
}
