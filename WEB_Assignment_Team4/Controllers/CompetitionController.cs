﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_Assignment_Team4.DAL;
using WEB_Assignment_Team4.Models;

namespace WEB_Assignment_Team4.Controllers
{
    public class CompetitionController : Controller
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private InterestDAL interestContext = new InterestDAL();

        // GET
        public ActionResult Index()
        {
            //Stop Accessing the action if not logged in
            //or account not in the "Administrator" role
            if (HttpContext.Session.GetString("Role") == null ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Competition> competitionList = competitionContext.GetAllCompetition();
            return View(competitionList);
        }
        private List<Interest> GetAllInterests()
        {
            List<Interest> interestList = interestContext.GetAllInterest();
            interestList.Insert(0, new Interest
            {
                AreaInterestID = 0,
                Name = "--Select--"
            });
            return interestList;
        }
        // GET: CompetitionController/Details/5
        public ActionResult Details(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            Competition competition = competitionContext.GetDetails(id);
            CompetitionViewModel competitionVM = MapToCompetitionVM(competition);
            return View(competitionVM);
        }

        public ActionResult List(int? id)
        {
            CompetitionViewModel competitionVM = new CompetitionViewModel();
            competitionVM.competitionList = competitionContext.GetAllCompetition();
            if (id != null)
            {
                ViewData["selectedCompetitionNo"] = id.Value;
                competitionVM.commentList = competitionContext.GetCompetitionComment(id.Value);
            }
            else
            {
                ViewData["selectedCompetitionNo"] = "";
            }
            return View(competitionVM);
        }


        public CompetitionViewModel MapToCompetitionVM(Competition competition)
        {
            string areaInterestName = "";
            if (competition.AreaInterestID != null)
            {
                List<Interest> interestList = interestContext.GetAllInterest();
                foreach (Interest interest in interestList)
                {
                    if(interest.AreaInterestID == competition.AreaInterestID.Value)
                    {
                       areaInterestName = interest.Name;
                       break;
                    }
                }
            }

            CompetitionViewModel competitionVM = new CompetitionViewModel
            {
                CompetitionID = competition.CompetitionID,
                Name = competition.Name,
                StartDate = competition.StartDate,
                EndDate = competition.EndDate,
                ResultReleaseDate = competition.ResultReleaseDate,
                AreaInterestName = areaInterestName
            };
            return competitionVM;
        }

        // GET: CompetitionController/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Administrator" Role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["interestList"] = GetAllInterests();
            return View();
        }

        // POST: CompetitionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competition competition)
        {
            //Get interest list for drop-down list
            //in case of the need to return to create.cshtml view
            ViewData["interestList"] = GetAllInterests();
            
            try
            {
                //Add competition records to database
                competition.CompetitionID = competitionContext.Add(competition);
                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                //Input validation fails, return to the Competition view
                //To display error message
                TempData["Message"] = "Select One Interest from the list. ";
                return RedirectToAction("Create");
            }
        }

        // GET: CompetitionController/Edit/5
        public ActionResult Edit(int? id)
        {
            //Stop accessing the action if not logged in
            //or account not in the "Administrator" role
            if ((HttpContext.Session.GetString("Role") == null) ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Competition");
            }
            if(id == null) //Query string parameter not provided 
            {
                //Return the listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            ViewData["interestList"] = GetAllInterests();
            Competition competition = competitionContext.GetDetails(id.Value);
            if( competition == null)
            {
                //Return the listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(competition);
        }

        // POST: CompetitionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Competition competition)
        {
            //Get interest List for the drop-down list
            //in case of the need to return to Edit.cshtml view
            ViewData["interestList"] = GetAllInterests();

            if (ModelState.IsValid)
            {
                //Update competition record to the database
                competitionContext.Update(competition);
                TempData["Message"] = "Records Updated Successfully. ";
                return RedirectToAction("Index");
            }
            else
            {
                //Input Validation fails, return to the view
                //to display an error message
                TempData["Message"] = "Records Update Failed. ";
                return View(competition);
            }
        }
        // POST: CompetitionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Competition Competition)
        {
            //Delete the records from the database
            competitionContext.Delete(Competition.CompetitionID);
            TempData["Message"] = "Records Added Successfully. ";
            return RedirectToAction("Index");
        }
        // GET: CompetitionController/Delete/5
        public ActionResult Delete(int? id)
        {
            //Stop accessing the action if not logged in
            //or account not in the "Administrator" role
            if ((HttpContext.Session.GetString("Role") == null) ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Competition");
            }
            if (id == null)
            {
                //Return to the index page, not allowed to edit
                return RedirectToAction("Index");
            }
            Competition competition = competitionContext.GetDetails(id.Value);
            if (competition == null)
            {
                //Return to the index page, not allowed to edit
                return RedirectToAction("Index");
            }
            competitionContext.Delete(competition.CompetitionID);
            return View(competition);
        }
    }
}
