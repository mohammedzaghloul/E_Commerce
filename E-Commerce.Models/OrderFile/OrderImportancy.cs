namespace E_Commerce.Models.OrderFile
{
    public class OrderImportancy
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public virtual ICollection<Order>? Orders { get; set; } = new HashSet<Order>();
    }
}
