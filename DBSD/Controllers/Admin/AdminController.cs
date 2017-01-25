using DBSD.Services;
using System;
using System.Web;
using System.Web.Mvc;
/*
*   This is controller that consist all methods used in Admin pages.
*
*   Created by Bobir Khaytmatov, Rifat Ahmed and Haoda Zhang for CQUniversity
*/
namespace DBSD.Controllers.Admin
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            Validate validation = new Validate();
            validation = ValidateAdmin();
            UserProperties NewUser = new UserProperties();
            if (validation.isAdmin == true) { NewUser.UserName = validation.GreetingName; } else { RedirectToAction("Index", "RegisterUser"); }
            return View("Index", NewUser);
        }

        public ActionResult GetProducts()
        {
            Validate validation = new Validate();
            validation = ValidateAdmin();
            if (validation.isAdmin == true)
            {
                var manager = new ProductDB();
                var items = manager.GetItems();
                return View("Products", items);
            }
                return RedirectToAction("Index", "Home");
        }
        public ActionResult GetJustProducts()
        {
           
                var manager = new ProductDB();
                var items = manager.GetItems();
                return View("Coffee", items);
        }

        public ActionResult AddNewProduct() {
            Validate validation = new Validate();
            validation = ValidateAdmin();
            if (validation.isAdmin == true)
            {
                return View("AddProduct");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddProduct(string productName, string productDescription, int productPrice, int productQuantity) {
            Validate validation = new Validate();
            validation = ValidateAdmin();
            if (validation.isAdmin == true)
            {
                ProductProperties NewProduct = new ProductProperties();
                NewProduct.ProductName = productName;
                NewProduct.ProductDescription = productDescription;
                NewProduct.ProductPrice = productPrice;
                NewProduct.ProductQuantity = productQuantity;
                var manager = new ProductDB();
                manager.AddItem(NewProduct);
            }
            return RedirectToAction("GetProducts", "Admin");
        }

        public ActionResult ConfirmDelete(Int64 id) {
            Validate validation = new Validate();
            validation = ValidateAdmin();
            if (validation.isAdmin == true)
            {
                var manager = new ProductDB();
                manager.RemoveItem(id);
                return RedirectToAction("GetProducts", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditProduct(Int64 id) {
            Validate validation = new Validate();
            validation = ValidateAdmin();
            if (validation.isAdmin == true)
            {
                var manager = new ProductDB();
            var items = manager.GetById(id);
            return View("EditProduct", items);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit(Int64 productId, string productName, string productDescription, int productPrice, int productQuantity) {
            Validate validation = new Validate();
            validation = ValidateAdmin();
            if (validation.isAdmin == true)
            {
                ProductProperties prod = new ProductProperties();
            prod.ProductId = productId;
            prod.ProductName = productName;
            prod.ProductDescription = productDescription;
            prod.ProductPrice = productPrice;
            prod.ProductQuantity = productQuantity;
            var manager = new ProductDB();
            manager.UpdateItem(prod);
            return RedirectToAction("GetProducts", "Admin");
        }
            return RedirectToAction("Index", "Home");
    }
        public Validate ValidateAdmin()
        {
            HttpCookie myCookie = Request.Cookies["MySuperToken"];
            Validate response = new Validate();
            if (String.IsNullOrWhiteSpace(myCookie.Value.ToString()))
            {
                response.isAdmin = false;
                response.GreetingName = "";
                return response;
            }
            else {
                var manager = new UserServices();
                var data = manager.CheckToken(myCookie.Value.ToString());
                if (data.Count != 0 && data[0] == "bob77")
                {
                    response.GreetingName = data[0];
                    response.isAdmin = true;
                    return response;
                }
                else
                {
                    response.GreetingName = "";
                    response.isAdmin = false;
                    return response;
                }
            }
        }
    }
}