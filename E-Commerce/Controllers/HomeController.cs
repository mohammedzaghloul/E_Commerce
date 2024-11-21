using E_Commerce.DataAccessDataAccess.Repository;
using E_Commerce.DataAccessDataAccess.Repository.IRepository;
using E_Commerce.Models;
using E_Commerce.Models.Product;
using E_Commerce.Models.ShoppingCartFile;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this._unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            IEnumerable<Category> categoryList = _unitOfWork.Category.GetAll();
            return View(categoryList);
        }
        public IActionResult ProductIndex(int?id)
        {
            IEnumerable<Product> ProductList = _unitOfWork.Product.GetAll(p=>p.CategoryId==id,includeProperties:"Category");
            return View(ProductList);
        }

        public IActionResult ProductItemIndex(int? id)
        {
            IEnumerable<ProductItem> ProductItemList = _unitOfWork.ProductItem.GetAll(p => p.ProductId == id, includeProperties: "Product");
            return View(ProductItemList);
        }

        public IActionResult ProductItemDetails(int productId)
        {

            ProductItem productItem = _unitOfWork.ProductItem.Get(p => p.Id == productId, includeProperties: "Product");

            ShoppingCart cart = new()
            {
                ProductItem = productItem,
                Quantity = 1,
                ProductItemId = productItem.Id
            };

            if (cart.ProductItem == null)
                return NotFound();

            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult ProductItemDetails(ShoppingCart shoppingCart)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicaitonUserId = UserId;

            /// Checking if the user add some product multiple times but in difference requests
            /// 

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(s => s.ApplicaitonUserId == UserId && s.ProductItemId == shoppingCart.ProductItemId);

            if (cartFromDb != null)
            {
                // Shopping Cart Exists
                cartFromDb.Quantity += shoppingCart.Quantity;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();

            }
            else
            {
                // Add Cart Record
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(s => s.ApplicaitonUserId == UserId).Count());
            }



            return RedirectToAction("Index"); // ActionName, ControllerName
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



       
    }
}
