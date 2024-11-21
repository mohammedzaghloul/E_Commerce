namespace E_Commerce.Models.OrderFile
{
    public class Tax
    {
        public int Id { get; set; }
        public int TaxRate { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
