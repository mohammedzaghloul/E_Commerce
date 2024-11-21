using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccessDataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IProductRepository Product { get; }
        public ICategoryRepository Category { get; }
        public IProductItemRepository ProductItem { get; }
        public IShoppingCartRepository ShoppingCart { get; }
        public IUserRepository User { get; }
        public IOrderRepository Order { get; }
        public IOrderLineRepository OrderLine { get; }

        void Save();
    }
}
