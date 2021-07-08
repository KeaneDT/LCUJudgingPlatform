using System;
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
            // or account not in the "Staff" Role
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
            
            if (ModelState.IsValid)
            {
                //Add competition records to database
                competition.CompetitionID = competitionContext.Add(competition);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Competition view
                //To display error message
                return View(competition);
            }
        }

        // GET: CompetitionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompetitionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompetitionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompetitionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
