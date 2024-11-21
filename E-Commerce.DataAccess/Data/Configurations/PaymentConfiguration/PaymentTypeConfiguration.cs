using E_Commerce.Models.Payment;

namespace E_Commerce.DataAccessData.Configurations.PaymentConfiguration
{
    public class PaymentTypeConfiguration : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();

        }
    }
}
