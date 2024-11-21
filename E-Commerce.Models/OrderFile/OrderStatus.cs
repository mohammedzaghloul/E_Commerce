using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models.OrderFile
{
    [NotMapped]
    public class OrderStatus
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Order>? Orders { get; set; } = new HashSet<Order>();
    }
}
