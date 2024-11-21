using E_Commerce.Models.OrderFile;

namespace E_Commerce.Models.UserFile
{
    public class UserAddress
    {
        public int Id { get; set; }

        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string State { get; set; }

        public string StreetAddress { get; set; }
        public string PostalCode { get; set; } = string.Empty;

        public int? UnitNumber { get; set; }

        public int GovernorateID { get; set; }
        public virtual ICollection<UserAddresses>? UserAddresses { get; set; } = new HashSet<UserAddresses>();
        public virtual ICollection<Order>? Orders { get; set; } = new HashSet<Order>();
        public virtual Governorate? Governorate { get; set; }
    }
}
