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
                return RedirectToAction ("AdminMain", "Home");
            }
            List<Interest> interestList = interestContext.GetAllInterest();
            return View(interestList);
        }
    }
}
