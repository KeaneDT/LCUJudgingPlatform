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
                return RedirectToAction ("Index", "Home");
            }
            List<Interest> interestList = interestContext.GetAllInterest();
            return View(interestList);
        }

        public ActionResult Select(int? id)
        {
            InterestViewModel interestVM = new InterestViewModel();
            interestVM.interestList = interestContext.GetAllInterest();
            if (id != null)
            {
                ViewData["selectedInterestNo"] = id.Value;
                interestVM.competitionList = interestContext.GetInterestCompetition(id.Value);
            }
            else
            {
                ViewData["selectedInterestNo"] = "";
            }
            return View(interestVM);
        }


        public ActionResult Create()
        {
            //Stop Accessing the action if not logged in
            //or account not in the "Administrator" role
            if (HttpContext.Session.GetString("Role") == null ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Interest");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Interest interest)
        {
            //
            interestContext.Delete(interest.AreaInterestID);
            return RedirectToAction("Index");
        }
        [HttpPost]
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
        public ActionResult Delete(int? id)
        {
            //Stop Accessing the action if not logged in
            //or account not in the "Administrator" role
            if (HttpContext.Session.GetString("Role") == null ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            Interest interest = interestContext.GetInterestDetails(id.Value);
            if(interest == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(interest);
        }
    }
}
