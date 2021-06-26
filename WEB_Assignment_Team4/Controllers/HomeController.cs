﻿using System;
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

        public ActionResult AdminLogin(IFormCollection FormData)
        {
            string userID = FormData["txtLoginID"].ToString();
            string password = FormData["txtPassword"].ToString();
            string Role = "Administrator";
            DateTime DateTiming = DateTime.Now;

            if(userID == "admin1@lcu.edu.sg" && password == "p@55Admin")
            {
                // Store Login ID in session with the key "LoginID"
                HttpContext.Session.SetString("LoginID", userID);
                // Store user role "Staff" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", Role);
                // Store date and time of the user when it has logged in
                HttpContext.Session.SetString("DateTiming", DateTiming.ToString());

                return RedirectToAction("AdminMain");
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
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
