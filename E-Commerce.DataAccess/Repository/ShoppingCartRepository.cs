using E_Commerce.DataAccess.Data;
using E_Commerce.DataAccessDataAccess.Repository.IRepository;
using E_Commerce.Models.Product;
using E_Commerce.Models.ShoppingCartFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccessDataAccess.Repository
{
    public class ShoppingCartRepository:Repository<ShoppingCart>,IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context):base(context) 
        {
            this._context = context;
        }


       
      
        public void Update(ShoppingCart shoppingCart)
        {
            _context.ShoppingCart.Update(shoppingCart);
        }
    }
}
