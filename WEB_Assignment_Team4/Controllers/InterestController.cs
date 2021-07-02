using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_Assignment_Team4.DAL;
using WEB_Assignment_Team4.Models;

namespace WEB_Assignment_Team4.Controllers
{
    public class InterestController : Controller
    {
        private InterestDAL interestContext = new InterestDAL();
        
        // GET: Interest
        public ActionResult Index()
        {
            //Stop Accessing the action if not logged in
            //or account not in the "Administrator" role
            if(HttpContext.Session.GetString("Role") == null ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction ("AdminMain", "Home");
            }
            List<Interest> interestList = interestContext.GetAllInterest();
            return View(interestList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Interest interest)
        {
            if (ModelState.IsValid)
            {
                // add interest records to database
                interest.AreaInterestID = interestContext.Add(interest);
                // Redirect user to Interest/Index view
                return RedirectToAction("Index");
            }
            else
            {
                // input validation fails, return to the Create view 
                // to display error messgae
                return View(interest);
            }
        }
    }
}
