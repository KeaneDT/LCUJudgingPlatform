using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WEB_Assignment_Team4.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
   
        public IActionResult PublicMain()
        {
            return View();
        }

        public ActionResult Login(IFormCollection FormData)
        {
            string userID = FormData["txtLoginID"].ToString();
            string password = FormData["txtPassword"].ToString();
            DateTime DateTiming = DateTime.Now;

            //Admin Login
            if(userID == "admin1@lcu.edu.sg" && password == "p@55Admin")
            {
                string Role = "Administrator";
                // Store Login ID in session with the key "LoginID"
                HttpContext.Session.SetString("LoginID", userID);
                // Store user role "Staff" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", Role);
                // Store date and time of the user when it has logged in
                HttpContext.Session.SetString("DateTiming", DateTiming.ToString());

                return RedirectToAction("AdminMain");
            }
            //Judge Login
            else if(userID == "abc1@lcu.edu.sg" && password == "p@55Judge")
            {
                string Role = "Judge";
                // Store Login ID in session with the key "LoginID"
                HttpContext.Session.SetString("LoginID", userID);
                // Store user role "Staff" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", Role);
                // Store date and time of the user when it has logged in
                HttpContext.Session.SetString("DateTiming", DateTiming.ToString());

                return RedirectToAction("JudgeMain");
            }
            else
            {
                // Store an error message to TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials";
                return RedirectToAction("PublicMain");
            }
        }
        public ActionResult AdminMain()
        {
            //Stop Accessing the action if not logged in
            //or account not in the "Administrator" role
            if (HttpContext.Session.GetString("Role") == null ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult JudgeMain()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();            
            // Call the Index action of Home controller
            return RedirectToAction("PublicMain");
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
