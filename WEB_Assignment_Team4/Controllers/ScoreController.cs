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
    public class ScoreController : Controller
    {
        //Declare DAL Objects to use SQL Commands in the Actions
        CriteriaDAL criteriaContext = new CriteriaDAL();
        CompetitionDAL competitionContext = new CompetitionDAL();
        CompetitorDAL competitorContext = new CompetitorDAL();
        SubmissionsDAL submissionsContext = new SubmissionsDAL();
        private List<SelectListItem> submissionsCount = new List<SelectListItem>();

        public ActionResult Index(int? id)
        {
            //Check if the Role of the user is a Judge
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }

            //Declare CompetitionSubmissionViewModel
            CompetitionSubmissionViewModel csVM = new CompetitionSubmissionViewModel();
            //Add the list of competitions the Judge is in through the use of SELECT commands
            csVM.competitionList = competitionContext.GetJudgeCompetition(HttpContext.Session.GetString("LoginID"));

            //Set the SelectedCompetitionNo to "" which means nothing is selected
            ViewData["selectedCompetitionNo"] = "";
            //Check the selected CompetitionID that has been passed to the query string
            if (id != null)
            {
                ViewData["selectedCompetitionNo"] = id.Value;
                ViewData["competitionName"] = competitionContext.GetDetails(id.Value).Name;
                //Set the Competition ID Selected to be used in other actions
                HttpContext.Session.SetInt32("criteriaSubNum", id.Value);
                // Get list of Criteria for the Competition
                csVM.submissionsList = submissionsContext.GetCompetitionSubmissions(id.Value);
            }
            else
            {
                ViewData["selectedCompetitionNo"] = "";
            }
            return View(csVM);
        }

        // GET: ScoreController/Details/5
        public ActionResult Details(int competitionID, int competitorID)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["competitionName"] = competitionContext.GetDetails(competitionID).Name;
            ViewData["resultsDate"] = competitionContext.GetDetails(competitionID).ResultReleaseDate;
            SubmissionViewModel sVM = submissionsContext.GetSubmissionDetails(competitionID, competitorID);
            sVM.Score = criteriaContext.GetSubmissionCriteriaTotal(competitionID, competitorID);
            return View(sVM);
        }

        // GET: ScoreController/Edit/5
        public ActionResult Score(int competitionID, int competitorID)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }

            SubmissionViewModel sVM = submissionsContext.GetSubmissionDetails(competitionID, competitorID);
            ViewData["appeal"] = sVM.Appeal;

            ViewData["competitionName"] = competitionContext.GetDetails(competitionID).Name;
            ViewData["totalScore"] = criteriaContext.GetSubmissionCriteriaTotal(competitionID, competitorID);
            ViewData["totalWeightage"] = criteriaContext.GetWeightageTotal(competitionID);
            ViewData["resultsDate"] = competitionContext.GetDetails(competitionID).ResultReleaseDate;
            ViewData["competitionID"] = competitionID;
            ViewData["competitorID"] = competitorID;

            return View(criteriaContext.GetSubmissionCriteria(competitionID, competitorID));
        }

        [HttpGet]
        public ActionResult ScoreEdit(int? competitionID, int? competitorID, int? criteriaID)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (competitionID == null || competitorID == null || criteriaID == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            if (DateTime.Now > competitionContext.GetDetails(competitionID.Value).ResultReleaseDate)
            {
                return RedirectToAction("Index");
            }
            CriteriaViewModel cVM = criteriaContext.GetSubmissionCriteriaDetail(competitionID.Value, competitorID.Value,  criteriaID.Value);

            SubmissionViewModel sVM = submissionsContext.GetSubmissionDetails(competitionID.Value, competitorID.Value);
            ViewData["appeal"] = sVM.Appeal;

            if (cVM == null)
            {
                return RedirectToAction("Index");
            }

            return View(cVM);
        }

        // POST: ScoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScoreEdit(CriteriaViewModel cVM)
        {
            SubmissionViewModel sVM = submissionsContext.GetSubmissionDetails(cVM.CompetitionID, cVM.CompetitorID);
            ViewData["appeal"] = sVM.Appeal;

            if (ModelState.IsValid)
            {                
                criteriaContext.UpdateCriteriaScore(cVM);
                TempData["Success"] = "Editing of Score Successful!";
                return RedirectToAction("Score", new
                {
                    competitionID = cVM.CompetitionID,
                    competitorID = cVM.CompetitorID
                });
            }
            else
            {
                //Input validation fails, return to the ScoreEdit view
                //to display error message
                TempData["Message"] = "";
                return View(cVM);
            }
        }

        [HttpGet]
        public ActionResult Rank(int? competitionID, int? competitorID)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (competitionID == null || competitorID == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            if (DateTime.Now > competitionContext.GetDetails(competitionID.Value).ResultReleaseDate)
            {
                return RedirectToAction("Details", new
                {
                    competitionID = competitionID.Value,
                    competitorID = competitorID.Value
                });
            }

            ViewData["competitionName"] = competitionContext.GetDetails(competitionID.Value).Name;
            SubmissionViewModel sVM = submissionsContext.GetSubmissionDetails(competitionID.Value, competitorID.Value);
            sVM.Score = criteriaContext.GetSubmissionCriteriaTotal(competitionID.Value, competitorID.Value);

            if (sVM == null)
            {
                return RedirectToAction("Details", new
                {
                    competitionID = competitionID,
                    competitorID = competitorID
                });
            }

            for (int i = 1; i <= submissionsContext.GetCompetitionSubmissionsCount(competitionID.Value); i++)
            {
                submissionsCount.Add(
                new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString(),
                });
            }
            submissionsCount.Add(
                new SelectListItem
                {
                    Value = "0",
                    Text = "NULL",
                });

            ViewData["submissionsCountList"] = submissionsCount;

            return View(sVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rank(SubmissionViewModel sVM)
        {
            for (int i = 1; i <= submissionsContext.GetCompetitionSubmissionsCount(sVM.CompetitionID); i++)
            {
                submissionsCount.Add(
                new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString(),
                });
            }
            submissionsCount.Add(
                new SelectListItem
                {
                    Value = null,
                    Text = "NULL",
                });

            ViewData["submissionsCountList"] = submissionsCount;
            ViewData["competitionName"] = competitionContext.GetDetails(sVM.CompetitionID).Name;

            if (ModelState.IsValid)
            {
                if (sVM.Ranking == null)
                {
                    competitorContext.UpdateCompetitorRanking(sVM);
                    return RedirectToAction("Details", new
                    {
                        competitionID = sVM.CompetitionID,
                        competitorID = sVM.CompetitorID
                    });
                }
                else
                {
                    if (competitorContext.CheckRankingUnique(sVM) != true)
                    {
                        competitorContext.UpdateCompetitorRanking(sVM);
                        TempData["Success"] = "Editing of Rank Successful!";
                        return RedirectToAction("Details", new
                        {
                            competitionID = sVM.CompetitionID,
                            competitorID = sVM.CompetitorID
                        });
                    }
                    else
                    {
                        TempData["Message"] = "Ranking Already Exists!";
                        return View(sVM);
                    }
                }
            }
            else
            {
                //Input validation fails, return to the Rank view
                //to display error message
                TempData["Message"] = "";
                return View(sVM);
            }
        }
    }
}
