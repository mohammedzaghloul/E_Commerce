using E_Commerce.Models.Favourite;
using  E_Commerce.Models.OrderFile;
using E_Commerce.Models.Payment;
using E_Commerce.Models.ShoppingCartFile;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce.Models.UserFile
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public int Age { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        public virtual ICollection<UserAddresses>? UserAddresses { get; set; }
        public virtual ICollection<Order>? Orders {  get; set; }
        public virtual ICollection<FavouriteListItems>? FavouriteListItems { get; set; } = new HashSet<FavouriteListItems>();

        public virtual ICollection<ProductItemReview> ProductItemReviews { get; set; } = new HashSet<ProductItemReview>();
        public virtual ICollection<UserPaymentMethod>? UserPaymentMethods { get; set; } = new HashSet<UserPaymentMethod>();

        [NotMapped]
        public string Role { get; set; }

        [NotMapped]
        public string FullName { get; set; }
    }
}
