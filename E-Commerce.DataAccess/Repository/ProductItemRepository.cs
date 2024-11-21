using E_Commerce.DataAccess.Data;
using E_Commerce.DataAccessDataAccess.Repository.IRepository;
using E_Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccessDataAccess.Repository
{
    public class ProductItemRepository : Repository<ProductItem>, IProductItemRepository
    {
        private readonly ApplicationDbContext db;

        public ProductItemRepository(ApplicationDbContext db):base(db)
        {
            this.db = db;
        }

        public void Update(ProductItem ProductItem)
        {
            db.ProductItems.Update(ProductItem);
        }
    }
}
