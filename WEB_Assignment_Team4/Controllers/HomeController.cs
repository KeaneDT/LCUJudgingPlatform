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

        public ActionResult StaffLogin(IFormCollection FormData)
        {
            string userID = FormData["txtLoginID"].ToString();
            string password = FormData["txtPassword"].ToString();
            string role = "Administrator";

            if(userID == "admin1@lcu.edu.sg" && password == "admin1@lcu.edu.sg")
            {
                return RedirectToAction("StaffMain");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult StaffMain()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
