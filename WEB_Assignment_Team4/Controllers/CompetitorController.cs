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
    public class CompetitorController : Controller
    {
        private InterestDAL interestContext = new InterestDAL();
        private CompetitorDAL competitorContext = new CompetitorDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();

        public ActionResult Create()
        {
            ViewData["SalutationList"] = GetSalutations();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competitor competitor)
        {
            //Get country list for drop-down list
            //in case of the need to return to Create.cshtml view
            ViewData["SalutationList"] = GetSalutations();
            if (ModelState.IsValid)
            {
                //Add staff record to database
                competitor.CompetitorID = competitorContext.Add(competitor);
                //Redirect user to Staff/Index view
                return RedirectToAction("PublicMain", "Home");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(competitor);
            }
        }
        private List<SelectListItem> GetSalutations()
        {
            List<SelectListItem> sal = new List<SelectListItem>();
            sal.Add(new SelectListItem
            {
                Value = "Dr",
                Text = "Dr"
            }); sal.Add(new SelectListItem
            {
                Value = "Mr",
                Text = "Mr"
            }); sal.Add(new SelectListItem
            {
                Value = "Ms",
                Text = "Ms"
            }); sal.Add(new SelectListItem
            {
                Value = "Mrs",
                Text = "Mrs"
            }); sal.Add(new SelectListItem
            {
                Value = "Mdm",
                Text = "Mdm"
            });

            return sal;
        }


    }
}


