using E_Commerce.Models.ViewModels;
using E_Commerce.Models.Product;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.DataAccessDataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; // Class-level variable

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this._webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View(unitOfWork.Product.GetAll(includeProperties:"Category").ToList());
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductWithCategorySelectListVM productVM = new()
            {
                Product = (id == 0 || id is not null) ? unitOfWork.Product.Get(p => p.Id == id, "Category") : new Product(),
                // categorylist
                CategoryList = unitOfWork.Category.GetAll().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
            };
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductWithCategorySelectListVM productVM, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/Products");
                string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadDir, fileName + extension);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream); // Synchronous operation
                }

                productVM.Product.ImageUrl = "/images/Products/" + fileName + extension;

                if (productVM.Product.Id != 0)
                {
                    //update
                    unitOfWork.Product.Update(productVM.Product);
                    TempData["Success"] = "Product Updated Successfully";

                }
                else
                {
                    // add
                    unitOfWork.Product.Add(productVM.Product);
                    TempData["Success"] = "Product created Successfully";
                }
               
                // save
                unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            // categorylist
            productVM.CategoryList = unitOfWork.Category.GetAll().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            return View(productVM);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Product product = unitOfWork.Product.Get(p => p.Id == id, "Category");
            if (product is null)
                return NotFound();
            
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int id)
        {
            // delete
            Product product = unitOfWork.Product.Get(c => c.Id == id);
            unitOfWork.Product.Remove(product);
            TempData["Success"] = "Product deleted Successfully";
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
