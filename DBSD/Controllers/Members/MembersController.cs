using DBSD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
/*
*   This is controller that consist all methods used in Member pages.
*
*   Created by Bobir Khaytmatov, Rifat Ahmed and Haoda Zhang for CQUniversity
*/
namespace DBSD.Controllers.Members
{
    public class MembersController : Controller
    {

        public ActionResult Index()
        {
            Validate status = new Validate();
            status = ValidateUser();
            if (status.isAdmin == true)
            {
                return RedirectToAction("Index", "Admin");
            }
            if (status.isUser == true) { 
            HttpCookie myCookie = Request.Cookies["MySuperToken"];
            var manager = new UserServices();
            List<string> data = new List<string>();
            data = manager.CheckToken(myCookie.Value.ToString());
            UserProperties NewUser = new UserProperties();
            if (data.Count != 0) { NewUser.UserName = data[0]; } else { RedirectToAction("Index", "RegisterUser"); }
            return View("Index", NewUser);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SeeProducts() {
            Validate status = new Validate();
            status = ValidateUser();
            if (status.isAdmin == true)
            {
                return RedirectToAction("Index", "Admin");
            }
            if (status.isUser == true)
            {
                var manager = new ProductDB();
                var items = manager.GetItems();
                return View("SeeProducts", items);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult OrderProduct(Int64 id) {
            HttpCookie myCookie = Request.Cookies["MySuperToken"];
            Validate status = new Validate();
            status = ValidateUser();
            if (status.isUser == true)
            {
                var productManager = new ProductDB();
                var userManager = new UserServices();
                var items = userManager.GetUserIdByToken(myCookie.Value.ToString());
                if (items.Count != 0){
                    UserProperties user = new UserProperties();
                    var data = userManager.CheckToken(myCookie.Value.ToString());
                    productManager.Order(id, items[0]);
                    var orderDetails = productManager.PlaceOrder(items[0]);
                    return View("Success", orderDetails);
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CompleteOrder() {
            HttpCookie myCookie = Request.Cookies["MySuperToken"];
            Validate status = new Validate();
            status = ValidateUser();
            if (status.isUser == true)
            {
                var manager = new UserServices();
                var id = manager.GetUserIdByToken(myCookie.Value.ToString());
                manager.CompleteOrder(id[0]);
                return View("OrderCompleted");
            }
            return RedirectToAction("Index", "Home");
        }

        public Validate ValidateUser()
        {
            HttpCookie myCookie = Request.Cookies["MySuperToken"];
            Validate response = new Validate();
            if (String.IsNullOrWhiteSpace(myCookie.Value.ToString()))
            {
                response.isAdmin = false;
                response.GreetingName = "";
                return response;
            }
            else
            {
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
                    response.isUser = true;
                    return response;
                }
            }
        }
    }
}
