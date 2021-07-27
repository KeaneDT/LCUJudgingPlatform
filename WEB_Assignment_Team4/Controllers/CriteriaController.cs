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
    public class CriteriaController : Controller
    {
        //Declare DAL Objects to use SQL Commands in the Actions
        CompetitionDAL competitionContext = new CompetitionDAL();
        CriteriaDAL criteriaContext = new CriteriaDAL();

        //GET Index Action used to display the CompetitionCriteriaViewModel. When a competition is selected, the respective criteria table will be displayed based on the CompetitionID
        public ActionResult Index(int? id)
        {
            //Check if the Role of the user is a Judge
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }

            //Declare CompetitionCriteriaViewModel
            CompetitionCriteriaViewModel ccVM = new CompetitionCriteriaViewModel();
            //Add the list of competitions the Judge is in through the use of SELECT commands
            ccVM.competitionList = competitionContext.GetJudgeCompetition(HttpContext.Session.GetString("LoginID"));

            //Set the SelectedCompetitionNo to "" which means nothing is selected
            ViewData["selectedCompetitionNo"] = "";
            //Check the selected CompetitionID that has been passed to the query string
            if (id != null)
            {
                ViewData["selectedCompetitionNo"] = id.Value;
                ViewData["competitionName"] = competitionContext.GetDetails(id.Value).Name;
                ViewData["totalWeightage"] = criteriaContext.GetCriteriaTotal(id.Value);
                //Set the Competition ID Selected to be used in other actions
                HttpContext.Session.SetInt32("criteriaCompNum", id.Value);
                // Get list of Criteria for the Competition
                ccVM.criteriaList = criteriaContext.GetCompetitionCriteria(id.Value);
            }
            else
            {
                ViewData["selectedCompetitionNo"] = "";
            }
            return View(ccVM);
        }

        //GET Action to display the View. The selected CompetitionID will be stored in the ViewData
        public ActionResult Create()
        {
            //Check if the Role of the user is a Judge
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["selectedCompetitionNo"] = HttpContext.Session.GetInt32("criteriaCompNum");
            return View();
        }

        //POST Action similar to GET Action but Values inserted are added to the Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Criteria criteria)
        {
            ViewData["selectedCompetitionNo"] = HttpContext.Session.GetInt32("criteriaCompNum");
            if (ModelState.IsValid)
            {   
                //Set the criteria's CompetitionID
                criteria.CompetitionID = Convert.ToInt32(HttpContext.Session.GetInt32("criteriaCompNum"));

                //Check if the total sum of the Criteria for the competition is less than 100
                if (criteriaContext.GetCriteriaTotal(criteria.CompetitionID) + criteria.Weightage <= 100)
                {
                    //Add the Criteria to the Criteria table for the specified Competition
                    criteria.CriteriaID = criteriaContext.Add(criteria);
                    //Redirect user to Criteria/Index view
                    return RedirectToAction("Index");
                }
                else
                {
                    //Error message if the total sum of the Criteria is more than 100
                    TempData["Message"] = "Total Weightage cannot be more than 100%!";
                    return View(criteria);
                }
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                TempData["Message"] = "";
                return View(criteria);
            }
        }

        //GET Action for Edit will display the View with the details of the Criteria Selected
        public ActionResult Edit(int? id)
        {
            //Check if the Role of the user is a Judge
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            //Get the details of the criteria specified in the Query String and assign it to criteria
            Criteria criteria = criteriaContext.GetDetails(id.Value);

            //If the criteria contains no details then redirect to Index
            if (criteria == null)
            {
                return RedirectToAction("Index");
            }
            //Store the SelectedCompetitionNo in ViewData
            ViewData["selectedCompetitionNo"] = HttpContext.Session.GetInt32("criteriaCompNum");
            return View(criteria);
        }

        //Post Action Similar to GET but the values put into the viw are validated and if allowed, will be updated in the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Criteria criteria)
        {
            //Get the Selected Competition number and store it in a ViewData
            ViewData["selectedCompetitionNo"] = HttpContext.Session.GetInt32("criteriaCompNum");
            if (ModelState.IsValid)
            {
                //Set the CompetitionID of the criteria
                criteria.CompetitionID = Convert.ToInt32(HttpContext.Session.GetInt32("criteriaCompNum"));
                //If the edited value of the Weightage added with the rest of the criteria weightage is less than 100
                if (criteriaContext.GetCriteriaTotal(criteria.CompetitionID) - criteriaContext.GetCriteria(criteria.CriteriaID) + criteria.Weightage <= 100)
                {
                    //Update the Values of the Criteria
                    criteria.CriteriaID = criteriaContext.Update(criteria);
                    //Redirect user to Criteria/Index view
                    return RedirectToAction("Index");
                }
                else
                {
                    //Error message if the total sum of the Criteria is more than 100
                    TempData["Message"] = "Total Weightage cannot be more than 100%!";
                    return View(criteria);
                }
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                TempData["Message"] = "";
                return View(criteria);
            }

        }

        //GET Action for Delete displays the View for the respective criteria selected
        public ActionResult Delete(int? id)
        {
            //Check if the Role of the user is a Judge
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["selectedCompetitionNo"] = HttpContext.Session.GetInt32("criteriaCompNum");
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            //Get the details of the criteria specified in the Query String and assign it to criteria
            Criteria criteria = criteriaContext.GetDetails(id.Value);

            //If the criteria contains no details then redirect to Index
            if (criteria == null)
            {
                return RedirectToAction("Index");
            }
            return View(criteria);
        }

        //POST Action for Delete just deletes the Criteria using the CriteriaID and SQL DELETE Commands
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Criteria criteria)
        {
            ViewData["selectedCompetitionNo"] = HttpContext.Session.GetInt32("criteriaCompNum");
            // Delete the staff record from database
            criteriaContext.Delete(criteria.CriteriaID);
            return RedirectToAction("Index");
        }
    }
}
