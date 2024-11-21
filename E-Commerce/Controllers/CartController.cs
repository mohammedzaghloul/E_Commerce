using E_Commerce.DataAccessDataAccess.Repository.IRepository;
using E_Commerce.Models.OrderFile;
using E_Commerce.Models.ShoppingCartFile;
using E_Commerce.Models.UserFile;
using E_Commerce.Models.ViewModels;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Stripe.Checkout;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    // i didn't put this in the area of customer so when you copy paste that to the customer area uncomment this
    //[Area("customer")]

    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicaitonUserId == UserId, includeProperties:"ProductItem"),
                Order=new()
               
                
            };
            
            /// Calculating the total price
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                
                cart.Price = cart.ProductItem.Price * cart.Quantity; // if you want to view every shopping cart item specific price
                ShoppingCartVM.Order.TotalPrice += (cart.ProductItem.Price * cart.Quantity);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicaitonUserId == UserId, includeProperties: "ProductItem"),
                Order = new Order()
            };

            ShoppingCartVM.Order.User = _unitOfWork.User.Get(u => u.Id == UserId);
            ShoppingCartVM.Order.Name = ShoppingCartVM.Order.User.FirstName + " " + ShoppingCartVM.Order.User.LastName;
            ShoppingCartVM.Order.PhoneNumber = ShoppingCartVM.Order.User.PhoneNumber;
            ShoppingCartVM.Order.StreetAddress = ShoppingCartVM.Order.User.StreetAddress;
            ShoppingCartVM.Order.City = ShoppingCartVM.Order.User.City;
            ShoppingCartVM.Order.State = ShoppingCartVM.Order.User.State;
            ShoppingCartVM.Order.PostalCode = ShoppingCartVM.Order.User.PostalCode;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {

                cart.Price = cart.ProductItem.Price * cart.Quantity; // if you want to view every shopping cart item specific price
                ShoppingCartVM.Order.TotalPrice += cart.Price;
            }
           
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicaitonUserId == UserId, includeProperties: "ProductItem");
            ShoppingCartVM.Order.OrderDate = System.DateTime.Now;
            ShoppingCartVM.Order.UserId = UserId;

            User Applicationuser = _unitOfWork.User.Get(u => u.Id == UserId);

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {

                cart.Price = cart.ProductItem.Price * cart.Quantity; // if you want to view every shopping cart item specific price
                ShoppingCartVM.Order.TotalPrice += (cart.ProductItem.Price * cart.Quantity);
            }
            ShoppingCartVM.Order.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.Order.OrderStatuss = SD.StatusPending;
            ShoppingCartVM.Order.AddressId = null;
            ShoppingCartVM.Order.ImportancyId = null;
            ShoppingCartVM.Order.OrderStatusId = null;
            ShoppingCartVM.Order.TaxId = null;
            ShoppingCartVM.Order.PaymentMethodId = null;

            if (ModelState.IsValid)
            {
                _unitOfWork.Order.Add(ShoppingCartVM.Order);
                _unitOfWork.Save();
            }
            else
            {
                return View(ShoppingCartVM);
            }
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderLine orderDetail = new()
                {
                    ProductItemId = cart.ProductItemId,
                    OrderId = ShoppingCartVM.Order.Id,
                    Price = cart.Price,
                    Quantity = cart.Quantity
                };
                if (ModelState.IsValid)
                {
                    _unitOfWork.OrderLine.Add(orderDetail);
                    _unitOfWork.Save();
                }
            }
            //it is a regular customer account and we need to capture payment
            //stripe logic
            //var domain = "https://localhost:44355/";
            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Cart/OrderConfirmation?id={ShoppingCartVM.Order.Id}",
                CancelUrl = domain + "Cart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.ProductItem.Price * 100), // $20.50 => 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductItem.SKU
                        }
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(sessionLineItem);

            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.Order.UpdateStripePaymentID(ShoppingCartVM.Order.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303); 

            //return RedirectToAction(nameof(OrderConfirmation), new {id=ShoppingCartVM.Order.Id});
        }

        public IActionResult OrderConfirmation(int id)
        {
            Order orderHeader = _unitOfWork.Order.Get(u => u.Id == id, includeProperties: "User");
            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                //this is an order by customer

                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.Order.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.Order.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }

                HttpContext.Session.Clear(); // To Clear the view of shopping cart to 0
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicaitonUserId == orderHeader.UserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }

        public IActionResult Plus(int CartId) 
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == CartId);
            cartFromDb.Quantity += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
           _unitOfWork.Save();
            TempData["Success"] = "Product Incremented Successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int CartId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == CartId, tracked:true);
            if(cartFromDb.Quantity <= 1)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicaitonUserId == cartFromDb.ApplicaitonUserId).Count() - 1);
                _unitOfWork.ShoppingCart.Remove(cartFromDb);

            }
            else
            {
                cartFromDb.Quantity -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                TempData["Success"] = "Product Decremented Successfully";
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int CartId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == CartId, tracked:true);
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicaitonUserId == cartFromDb.ApplicaitonUserId).Count()-1);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

    }
}
