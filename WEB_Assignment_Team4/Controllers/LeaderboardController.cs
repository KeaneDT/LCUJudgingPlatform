using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_Assignment_Team4.DAL;
using WEB_Assignment_Team4.Models;

namespace WEB_Assignment_Team4.Controllers
{
    public class LeaderboardController : Controller
    {
        private InterestDAL interestContext = new InterestDAL();
        private SubmissionsDAL submissionContext = new SubmissionsDAL();

        // GET: LeaderboardController
        public ActionResult Index(int? id)
        {
            Submissions submissions = new Submissions();
            if (id != null)
            {
                ViewData["selectedCompetition"] = id.Value;
                submissions.submissionsList = submissionContext.GetCompetitionSubmissionsLeaderboard(id.Value);
            }
            else
            {
                ViewData["selectedCompetition"] = "";
            }
            return View(submissions);
        }

        // GET: LeaderboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LeaderboardController/Create
        public ActionResult Create()
        {
            return View();
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

        // POST: LeaderboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: LeaderboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaderboardController/Edit/5
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

        // GET: LeaderboardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaderboardController/Delete/5
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
