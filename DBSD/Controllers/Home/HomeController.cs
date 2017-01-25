using System;
using System.Web.Mvc;
using DBSD.Services;
using System.Web;
using System.Collections.Generic;
/*
*   This is controller that consist all methods used in all not Admin/Member pages.
*
*   Created by Bobir Khaytmatov, Rifat Ahmed and Haoda Zhang for CQUniversity
*/
namespace DBSD.Controllers.Home
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            HttpCookie myCookie = Request.Cookies["MySuperToken"];
            if (myCookie != null)
            {
                var manager = new UserServices();
                List<string> data = new List<string>();
                data = manager.CheckToken(myCookie.Value.ToString());
                if (data.Count != 0) { if (data[0] == "bob77") { return RedirectToAction("Index", "Admin"); }
                    else { return RedirectToAction("Index","Members"); } } }
             return View(); 
        }
        public ActionResult RegisterUser() {
            return View("RegisterUser");
        }
        public ActionResult Contact()
        {
            return View("Contact");
        }
        public ActionResult About()
        {
            return View("About");
        }
        public ActionResult Coffee()
        {
            var manager = new ProductDB();
            var items = manager.GetItems();
            return View("Products", items);
            
        }

        [HttpPost]
        public ActionResult NewUser(string username, string fname, string lname, string password1, string password2, string email, string phoneNum) {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(fname) || String.IsNullOrEmpty(password1) || String.IsNullOrEmpty(password2) || String.IsNullOrEmpty(phoneNum)) {
                return RedirectToAction("RegisterUser", "Home");
            }
            if (password1 != password2) {
                return RedirectToAction("RegisterUser", "Home");
            }
            UserProperties NewUser = new UserProperties();
            var manager = new UserServices();
            NewUser.UserName = username;
            NewUser.LastName = lname;
            NewUser.FirstName = fname;
            NewUser.Email = email;
            NewUser.Phone = phoneNum;
            NewUser.Token = manager.RandomString();
            NewUser.Password = password1;
            var result = manager.AddItem(NewUser);
            if (result == "Success")
            {
                HttpCookie myCookie = new HttpCookie("MySuperToken");
                DateTime now = DateTime.UtcNow;
                myCookie.Value = NewUser.Token;
                myCookie.Expires = now.AddHours(1);
                Response.Cookies.Add(myCookie);
                return RedirectToAction("Index", NewUser);
            }
            else
            {
                return View("RegisterUser");
            }
        }

        public ActionResult LoginView() {
            return View("Login");
        }
        public ActionResult Login(string username, string password) {
            var manager = new UserServices();
            List<string> data = new List<string>();
            data = manager.Login(username, password);
            if (data.Count==0) { return RedirectToAction("RegisterUser", "Home"); }
            UserProperties NewUser = new UserProperties();
            NewUser.Token = manager.RandomString();
            NewUser.FirstName = data[0];
            NewUser.UserName = username;
            HttpCookie myCookie = new HttpCookie("MySuperToken");
            DateTime now = DateTime.UtcNow;
            myCookie.Value = NewUser.Token;
            myCookie.Expires = now.AddHours(1);
            Response.Cookies.Add(myCookie);
            manager.WriteToken(NewUser);
            return RedirectToAction("Index", NewUser);
        }

        public ActionResult LogOut() {
            if (Request.Cookies["MySuperToken"] != null)
            {
                Response.Cookies["MySuperToken"].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Index", "Home");
        }
    }

}