using E_Commerce.DataAccessDataAccess.Repository.IRepository;
using E_Commerce.Models.Product;
using E_Commerce.Models.ShoppingCartFile;
using E_Commerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class ProductItemController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; // Class-level variable

        public ProductItemController(IUnitOfWork _unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = _unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(unitOfWork.ProductItem.GetAll(includeProperties: "Product").ToList());
        }

        /// Update and Insert
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductItemWithProductSelectListVM ProductItemVM = new ProductItemWithProductSelectListVM()
            {
                ProductItem = (id == 0 || id is not null) ? unitOfWork.ProductItem.Get(pi => pi.Id == id, "Product") : new ProductItem(),
                ProductList = unitOfWork.Product.GetAll().Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() })

            };
            return View(ProductItemVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductItemWithProductSelectListVM ProductItemVM, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/ProductItems");
                string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadDir, fileName + extension);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream); // Synchronous operation
                }

                ProductItemVM.ProductItem.ImageUrl = "/images/ProductItems/" + fileName + extension;
                // update
                if (ProductItemVM.ProductItem.Id != 0)
                {
                    unitOfWork.ProductItem.Update(ProductItemVM.ProductItem);
                    TempData["Success"] = "Product Item Updated Successfully";
                }
                else
                {
                    // add
                    unitOfWork.ProductItem.Add(ProductItemVM.ProductItem);
                    TempData["Success"] = "Product Item created Successfully";

                }

                unitOfWork.Save();
                return RedirectToAction("Index");

            }
            /// Product List
            ProductItemVM.ProductList = unitOfWork.Product.GetAll().Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });

            return View(ProductItemVM);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ProductItem productItem = unitOfWork.ProductItem.Get(pi => pi.Id == id, "Product");
            if(productItem is null)
            {
                return NotFound();
            }
            return View(productItem);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProductItem(int id)
        {
            ProductItem productItem = unitOfWork.ProductItem.Get(pi => pi.Id == id);
            unitOfWork.ProductItem.Remove(productItem);
            TempData["Success"] = "Product Item deleted Successfully";
            unitOfWork.Save();
            return RedirectToAction("Index");

        }

        public IActionResult Details(int Id)
        {
            ProductItem P = unitOfWork.ProductItem.Get(p => p.Id == Id, "Product");
            if (P is null)
                return NotFound();


            return View(P);
        }

    }
}
