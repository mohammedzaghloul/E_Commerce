using E_Commerce.DataAccess.Data;
using E_Commerce.DataAccessDataAccess.Repository.IRepository;
using E_Commerce.Models.OrderFile;
using E_Commerce.Models.ShoppingCartFile;
using E_Commerce.Models.UserFile;
using E_Commerce.Models.ViewModels;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Stripe.Checkout;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace E_Commerce.Controllers
{

    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationDbContext db; // for accessing the ASPNETRole table

        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            this.unitOfWork = unitOfWork;
            this.db = db;
        }

        public IActionResult Index()
        {
            List<User> users = unitOfWork.User.GetAll().ToList();
            var userRoles = db.UserRoles.ToList();
            var roles = db.Roles.ToList();
            foreach (var user in users)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                user.FullName = user.FirstName + " " + user.LastName;
            }


            return View(users);
        }

        
        public IActionResult LockUnlock(string id)
        {
            var objFromDb = unitOfWork.User.Get(u => u.Id == id, tracked:true);
            if(objFromDb == null)
            {
                return RedirectToAction("Index");
            }

            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                // user is currently locked and we should unlock
                objFromDb.LockoutEnd = null;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddDays(30);
            }
            unitOfWork.Save();
            return RedirectToAction("Index");

        }

        public IActionResult Edit(string id)
        {
            var objFromDb = unitOfWork.User.Get(u => u.Id == id, tracked: true);
            objFromDb.FullName = objFromDb.FirstName + " " + objFromDb.LastName;
            ViewBag.Roles = db.Roles.ToList();
            return View(objFromDb);
        }

        [HttpPost]
        [ActionName("Edit")]
        public IActionResult ConfirmEdit(string id, string Role)
        {

            if (Role == "0")
            {
                var objFromDb = unitOfWork.User.Get(u => u.Id == id, tracked: true);
                ModelState.AddModelError("DeptId", "Select Role");
                objFromDb.FullName = objFromDb.FirstName + " " + objFromDb.LastName;
                ViewBag.Roles = db.Roles.ToList();
                return View("Edit", objFromDb);
            }

            var userRoles = db.UserRoles.ToList();
            var userRole = userRoles.FirstOrDefault(UR => UR.UserId == id); // We Will Delete This Role and add a new role to the customer
            if (userRole != null)
            {
                // Remove the current role 
                db.UserRoles.Remove(userRole);
                db.SaveChanges();
            }

            // Add a new UserRole with the updated role
            var newUserRole = new IdentityUserRole<string>
            {
                UserId = id,
                RoleId = Role
            };

            TempData["Success"] = "User Role Updated Successfully";
            db.UserRoles.Add(newUserRole);
            db.SaveChanges(); 

            return RedirectToAction("Index");

        }
        #region API calling
        [HttpGet]
        public IActionResult GetAll()
        {
            List<User> Users = unitOfWork.User.GetAll().ToList();
            return Json(new { data = Users });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            return Json(new { success = true, message = "Delete Successful" });

        } 
        #endregion

    }
}
