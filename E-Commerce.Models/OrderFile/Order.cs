using E_Commerce.Models.Payment;
using E_Commerce.Models.UserFile;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models.OrderFile
{
    //Order Header
    public class Order
    {
        public int Id { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public double TotalPrice { get; set; }
        public string? OrderStatuss { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateOnly PaymentDueDate { get; set; }

        public string? SessionId { get; set; }

        public string? PaymentIntentId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Name { get; set; }

        public int? AddressId { get; set; }

        public int? TaxId { get; set; }
        public int? PaymentMethodId { get; set; }
        public int? ImportancyId { get; set; }

        public int? OrderStatusId { get; set; }
        public virtual ICollection<OrderLine>? OrderLines { get; set; } = new HashSet<OrderLine>();
        public virtual UserPaymentMethod? UserPaymentMethod { get; set; }
        public virtual UserAddress? UserAddress { get; set; }
        public virtual OrderStatus? OrderStatus { get; set; }
        public virtual OrderImportancy? OrderImportancy { get; set; }
        public virtual Tax? Tax { get; set; }

    }
}
