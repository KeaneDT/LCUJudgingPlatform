using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Assignment_Team4.DAL;
using WEB_Assignment_Team4.Models;

namespace WEB_Assignment_Team4.Controllers
{
    public class HomeController : Controller
    {
        private JudgeDAL judgeContext = new JudgeDAL();
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
            else if(judgeContext.ValidJudgeLogin(userID,password)==true)
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

        public IActionResult Voting()
        {
            return View();
        }
    }
}
