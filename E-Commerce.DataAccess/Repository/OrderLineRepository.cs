using E_Commerce.DataAccess.Data;
using E_Commerce.DataAccessDataAccess.Repository.IRepository;
using E_Commerce.Models.OrderFile;
using E_Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccessDataAccess.Repository
{
    public class OrderLineRepository:Repository<OrderLine>, IOrderLineRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderLineRepository(ApplicationDbContext context):base(context) 
        {
            this._context = context;
        }


       
      
        public void Update(OrderLine obj)
        {
            _context.OrderLines.Update(obj);
        }
    }
}
