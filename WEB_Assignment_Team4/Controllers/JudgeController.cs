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
    public class JudgeController : Controller
    {
        private InterestDAL interestContext = new InterestDAL();
        private JudgeDAL judgeContext = new JudgeDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();

        public ActionResult Create()
        {
            ViewData["SalutationList"] = GetSalutations();
            ViewData["InterestList"] = GetAllInterest();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Judge judge)
        {
            //Get country list for drop-down list
            //in case of the need to return to Create.cshtml view
            ViewData["SalutationList"] = GetSalutations();
            ViewData["InterestList"] = GetAllInterest();
            if (ModelState.IsValid)
            {
                //Add staff record to database
                judge.JudgeID = judgeContext.Add(judge);
                //Redirect user to Staff/Index view
                return RedirectToAction("PublicMain", "Home");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(judge);
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
        private List<Interest> GetAllInterest()
        {
            // Get a list of branches from database
            List<Interest> interestList = interestContext.GetAllInterest();
            // Adding a select prompt at the first row of the branch list
            interestList.Insert(0, new Interest
            {
                AreaInterestID = 0,
                Name = "--Select--"
            });
            return interestList;
        }
    }
}
