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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;

        public IProductRepository Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IProductItemRepository ProductItem { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IUserRepository User { get; private set; }
        public IOrderLineRepository OrderLine { get; private set; }
        public IOrderRepository Order { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            Product = new ProductRepository(db);
            Category = new CategoryRepository(db);
            ProductItem = new ProductItemRepository(db);
            ShoppingCart = new ShoppingCartRepository(db);
            User = new UserRepository(db);
            OrderLine = new OrderLineRepository(db);
            Order = new OrderRepository(db);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
