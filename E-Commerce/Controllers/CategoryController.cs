using E_Commerce.DataAccessDataAccess.Repository.IRepository;
using E_Commerce.Models.Product;
using Microsoft.AspNetCore.Authorization;

using E_Commerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; // Class-level variable
        public CategoryController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Category> CategoryList = _unitOfWork.Category.GetAll().ToList();
          
            return View(CategoryList);
        }

        public IActionResult Upsert(int? id)
        {
            CategoryVM categoryVM = new()
            {
                Category = (id == 0 || id !=null) ? _unitOfWork.Category.Get(c => c.Id == id, "ParentCategory") : new Category(),
                CategoryList = _unitOfWork.Category.GetAll(includeProperties:"ParentCategory").Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                }),

            };
            
            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CategoryVM categoryVM, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/Category");
                string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadDir, fileName + extension);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream); // Synchronous operation
                }

                categoryVM.Category.ImageUrl = "/images/Category/" + fileName + extension;

                if (categoryVM.Category.Id != 0)
                {
                    _unitOfWork.Category.Update(categoryVM.Category);
                    TempData["success"] = "Category Updated Successfully";
                    
                }
                else
                {
                    _unitOfWork.Category.Add(categoryVM.Category);
                    TempData["success"] = "Category Created Successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");

            }
            categoryVM.CategoryList = _unitOfWork.Category.GetAll(includeProperties: "ParentCategory").Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Name
            });

            return View(categoryVM);
        }
        
        
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _unitOfWork.Category.Get(c => c.Id == id, "ParentCategory");
          
            
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var categoryFromDb = _unitOfWork.Category.Get(c => c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(categoryFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";

            return RedirectToAction("Index");

        }
    }
}
