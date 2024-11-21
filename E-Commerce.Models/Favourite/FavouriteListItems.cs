using E_Commerce.Models.Product;
using E_Commerce.Models.UserFile;

namespace E_Commerce.Models.Favourite
{
    public class FavouriteListItems
    {
        public string UserId { get; set; }
        public int ProductItemId { get; set; }
        public virtual ProductItem? ProductItem { get; set; }
        public virtual User? User { get; set; }
    }
}
