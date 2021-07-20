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
    public class AssignController : Controller
    {
        private JudgeDAL judgeContext = new JudgeDAL();
        
        // GET: AssignController
        public ActionResult Index()
        {
            // Stop Accessing the action if not logged in
            //or account not in the "Administrator" role
            if (HttpContext.Session.GetString("Role") == null ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Judge> judgeList = judgeContext.GetAllJudges();
            return View(judgeList);
        }
     
        private List<Judge> GetAllJudges()
        {
            List<Judge> judgeList = judgeContext.GetAllJudges();
            judgeList.Insert(0, new Judge
            {
                JudgeID = 0,
                JudgeName = "--Select--"
            });
            return judgeList;
        }

        // GET: AssignController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AssignController/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Administrator" Role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["judgeList"] = GetAllJudges();
            return View();
        }

        // POST: AssignController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JudgeAssign judgeAssign)
        {
            //Get interest list for drop-down list
            //in case of the need to return to create.cshtml view
            ViewData["judgeList"] = GetAllJudges();

            try
            {
                //Add competition records to database
                //judgeAssign.CompetitionID = judgeContext.Add(judgeAssign);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //Input validation fails, return to the Competition view
                //To display error message
                TempData["Message"] = "Select One Interest from the list. ";
                return RedirectToAction("Create");
            }
        }

        // GET: AssignController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AssignController/Edit/5
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

        // GET: AssignController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AssignController/Delete/5
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
