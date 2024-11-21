using E_Commerce.Models.OrderFile;
using E_Commerce.Models.UserFile;

namespace E_Commerce.Models.Payment
{
    public class UserPaymentMethod
    {
        public int Id { get; set; }
        public string Provider { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public DateOnly ExpireDate { get; set; }
        public bool IsDefault { get; set; }




        //[ForeignKey("PaymentType")]
        public int PaymentTypeId { get; set; }
        //[ForeignKey("User")]
        public string UserId { get; set; }

        /// Navigation Properties
        public virtual PaymentType? PaymentType { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Order>? Orders { get; set; } = new HashSet<Order>();
    }
}
