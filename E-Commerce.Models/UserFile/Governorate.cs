namespace E_Commerce.Models.UserFile
{
    public class Governorate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderPrice { get; set; }

        public virtual ICollection<UserAddress>? UserAddresses { get; set; } = new HashSet<UserAddress>();
    }
}
