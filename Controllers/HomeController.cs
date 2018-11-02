using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsCategories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace ProductsCategories.Controllers
{
    public class HomeController : Controller
    {
        private ProductsCategoriesContext _context;
    
        public HomeController(ProductsCategoriesContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }
        [HttpPost("AddUser")]
        public IActionResult AddUser(UsersValidate uservalidator)
        {
            if(ModelState.IsValid)
            {

                var emailvalidation = _context.users.SingleOrDefault(p => p.email == uservalidator.email);
                if(emailvalidation == null)
                {
                    PasswordHasher<UsersValidate> Hasher = new PasswordHasher<UsersValidate>();
                    uservalidator.password = Hasher.HashPassword(uservalidator, uservalidator.password);
                    Users myUser = new Users();
                    myUser.first_name = uservalidator.first_name;
                    myUser.last_name = uservalidator.last_name;
                    myUser.email = uservalidator.email;
                    myUser.password = uservalidator.password;
                    myUser.created_at = DateTime.Now;
                    myUser.updated_at = DateTime.Now;
                    _context.Add(myUser);
                    _context.SaveChanges();

                    HttpContext.Session.SetInt32("UserID", myUser.id);
                    int? UserID = HttpContext.Session.GetInt32("UserID");
                    ViewBag.UserID = UserID;
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    TempData["uniqueemail"] = "This email belongs to a registered user. Please use another email address";
                    return View("Index");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("LoginProcess")]
        public IActionResult LoginProcess(LoginValidate myLogin)
        {
            if(ModelState.IsValid)
            {
                Users loginData = _context.users.SingleOrDefault(p => p.email == myLogin.login_email);
                if(loginData == null)
                {
                    ModelState.AddModelError("login_email", "Email Address is not registered");
                }
                else if(loginData != null && myLogin.login_password != null)
                {
                    var Hasher = new PasswordHasher<Users>();
                    // Pass the user object, the hashed password, and the PasswordToCheck
                    if(0 != Hasher.VerifyHashedPassword(loginData, loginData.password, myLogin.login_password))
                    {
                        HttpContext.Session.SetInt32("UserID", loginData.id);
                        int? UserID = HttpContext.Session.GetInt32("UserID");
                        ViewBag.UserID = UserID;
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("login_password", "Incorrect password");
                        return View("Index");
                    }
                }
                return View("Index");
            }
            // ViewBag.error = "LOL, Nice try!";
            // TempData["error"] = "LOL, try again!";
            return View("Index");
        }
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            ViewBag.userInfo = _context.users.Where(p => p.id == UserID).SingleOrDefault();
            // ViewBag.allSecrets = _context.secrets.Include(p => p.Like).ToList();
            return View("Dashboard");
        }
        [HttpGet("Product")]
        public IActionResult Product()
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            ViewBag.allProducts = _context.products.OrderBy(p => p.name).ToList();
            return View("Product");
        }
        [HttpGet("Category")]
        public IActionResult Category()
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            ViewBag.allCategories = _context.categories.OrderBy(p => p.name).ToList();
            return View("Category");
        }
        [HttpGet("Product/{pid}")]
        public IActionResult ProductType(int pid)
        {
            ViewBag.productDisplay = _context.products.Where(p => p.id == pid).Include(p => p.CategoriesProduct)
            .ThenInclude(p => p.Categories).SingleOrDefault();
            ViewBag.categoryDisplay = _context.categories.OrderBy(p => p.name).ToList();
            return View("ProductType");
        }
        [HttpPost("AddProduct")]
        public IActionResult AddProduct(ProductsValidation productvalidator)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            if(ModelState.IsValid)
            {
                Products myProduct = new Products();
                myProduct.name = productvalidator.name;
                myProduct.description = productvalidator.description;
                myProduct.price = productvalidator.price;
                myProduct.created_at = DateTime.Now;
                myProduct.updated_at = DateTime.Now;
                myProduct.usersid = (int)UserID;
                _context.Add(myProduct);
                _context.SaveChanges();
                return RedirectToAction("Product");
            }
            else
            {
                ViewBag.allProducts = _context.products.OrderBy(p => p.name).ToList();
                // TempData["error"] = "All three fields are required";
                return View("Product");
            }
        }
        [HttpPost("DeleteProduct")]
        public IActionResult DeleteProduct(int productID)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            var DelCatProd = _context.categoriesproducts.Where(p => p.productsid == productID).ToList();
            var DelProduct = _context.products.Where(p => p.id == productID).SingleOrDefault();
            foreach (var item in DelCatProd)
            {
                _context.categoriesproducts.Remove(item);
            }
            _context.Remove(DelProduct);
            _context.SaveChanges();
            return RedirectToAction("Product");
        }
        [HttpPost("AddCategory")]
        public IActionResult AddCategory(CategoriesValidation categoryvalidator)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            var categoryNameValidator = _context.categories.SingleOrDefault(p => p.name == categoryvalidator.name);
            if (categoryNameValidator == null)
            {
                Categories myCategory = new Categories();
                myCategory.name = categoryvalidator.name;
                myCategory.created_at = DateTime.Now;
                myCategory.updated_at = DateTime.Now;
                myCategory.usersid = (int)UserID;
                _context.Add(myCategory);
                _context.SaveChanges();
                return RedirectToAction("Category");
            }
            else
            {
                TempData["uniquename"] = "Please use a unique name";
                return View("Category");
            }
        }
        [HttpPost("DeleteCategory")]
        public IActionResult DeleteCategory(int categoryID)
        {
            var DelCatProd = _context.categoriesproducts.Where(p => p.categoriesid == categoryID).ToList();
            var DelCategory = _context.categories.Where(p => p.id == categoryID).SingleOrDefault();
            foreach (var item in DelCatProd)
            {
                _context.categoriesproducts.Remove(item);
            }
            _context.Remove(DelCategory);
            _context.SaveChanges();
            return RedirectToAction("Category");
        }
        [HttpPost("AddCatProd")]
        public IActionResult AddCatProd(int productID, int categoryID)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            CategoriesProducts myCatProd = new CategoriesProducts();
            myCatProd.productsid = productID;
            myCatProd.categoriesid = categoryID;
            _context.categoriesproducts.Add(myCatProd);
            _context.SaveChanges();
            return RedirectToAction("ProductType", new{pid=productID});
        }
        [HttpPost("DeleteCatProd")]
        public IActionResult DeleteCatProd(int productID, int categoryID)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            var DelCatProd = _context.categoriesproducts.Where(p => p.productsid == productID)
            .Where(p => p.categoriesid == categoryID).SingleOrDefault();
            _context.categoriesproducts.Remove(DelCatProd);
            _context.SaveChanges();
            return RedirectToAction("ProductType", new{pid=productID});
        }
    }
}
