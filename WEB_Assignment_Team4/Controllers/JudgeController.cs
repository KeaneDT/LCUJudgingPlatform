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
        //Declare DAL Objects to use SQL Commands in the Actions
        private InterestDAL interestContext = new InterestDAL();
        private JudgeDAL judgeContext = new JudgeDAL();

        //GET Action to display the View along with the Lists for Salutation and Interest defined
        public ActionResult Create()
        {
            ViewData["SalutationList"] = GetSalutations();
            ViewData["InterestList"] = GetAllInterest();
            return View();
        }

        //POST Action similar to GET Action but Values inserted are added to the Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Judge judge)
        {
            //Lists for the View
            ViewData["SalutationList"] = GetSalutations();
            ViewData["InterestList"] = GetAllInterest();
            if (ModelState.IsValid)
            {
                //If the user has not selected any Interest
                if (judge.AreaInterestID == 0)
                {
                    //Return View with Error Message
                    TempData["Message"] = "Select Area of Interest!";
                    return View(judge);
                }
                else
                {
                    //Add Judge record to database
                    judge.JudgeID = judgeContext.Add(judge);
                    //Redirect user to Home/PublicMain (Login Page) view
                    return RedirectToAction("PublicMain", "Home");
                }
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(judge);
            }
        }

        //Function to Create a list for Salutations & Populate the list with the relevant values
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

        //Function to populate a list of Interests using SQL Select commands
        private List<Interest> GetAllInterest()
        {
            // Get a list of Interests from database
            List<Interest> interestList = interestContext.GetAllInterest();
            // Adding a select prompt at the first row of the Interest list
            interestList.Insert(0, new Interest
            {
                AreaInterestID = 0,
                Name = "Select Interest"
            });
            return interestList;
        }
    }
}
