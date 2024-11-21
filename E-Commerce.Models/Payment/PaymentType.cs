namespace E_Commerce.Models.Payment
{
    public class PaymentType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<UserPaymentMethod>? PaymentMethods { get; set; } = new HashSet<UserPaymentMethod>();
    }
}
