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
using Microsoft.AspNetCore.Authentication.Cookies;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Google.Apis.Auth;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace WEB_Assignment_Team4.Controllers
{
    public class HomeController : Controller
    {
        private JudgeDAL judgeContext = new JudgeDAL();
        public IActionResult Index()
        {
            ViewData["Role"] = HttpContext.Session.GetString("Role");
            return View();
        }

        private CompetitorDAL competitorContext = new CompetitorDAL();
        public IActionResult Competitor()
        {
            return View();
        }
   
        public IActionResult PublicMain()
        {
            if ((HttpContext.Session.GetString("Role") == "Administrator") ||
            (HttpContext.Session.GetString("Role") == "Judge") ||
            (HttpContext.Session.GetString("Role") == "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Login(IFormCollection FormData)
        {
            if ((HttpContext.Session.GetString("Role") == "Administrator") ||
            (HttpContext.Session.GetString("Role") == "Judge") ||
            (HttpContext.Session.GetString("Role") == "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }

            string userID = FormData["txtLoginID"].ToString();
            string password = FormData["txtPassword"].ToString();
            DateTime DateTiming = DateTime.Now;

            //Admin Login
            if(userID == "admin1@lcu.edu.sg" && password == "p@55Admin")
            {
                string Role = "Administrator";
                // Store Login ID in session with the key "LoginID"
                HttpContext.Session.SetString("LoginID", userID);
                // Store user role "Staff" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", Role);
                // Store date and time of the user when it has logged in
                HttpContext.Session.SetString("DateTiming", DateTiming.ToString());

                return RedirectToAction("AdminMain");
            }
            //Judge Login
            else if(judgeContext.ValidJudgeLogin(userID,password)==true)
            {
                string Role = "Judge";
                // Store Login ID in session with the key "LoginID"
                HttpContext.Session.SetString("LoginID", userID);
                // Store user role "Judge" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", Role);
                // Store date and time of the user when it has logged in
                HttpContext.Session.SetString("DateTiming", DateTiming.ToString());

                return RedirectToAction("JudgeMain");
            }
            //Competitor Login
            else if (competitorContext.ValidCompetitorLogin(userID, password) == true)
            {
                string Role = "Competitor";
                // Store Login ID in session with the key "LoginID"
                HttpContext.Session.SetString("LoginID", userID);
                // Store user role "Competitor" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", Role);
                // Store date and time of the user when it has logged in
                HttpContext.Session.SetString("DateTiming", DateTiming.ToString());

                return RedirectToAction("CompetitorMain");
            }
            else
            {
                // Store an error message to TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials";
                return RedirectToAction("PublicMain");
            }
        }
        public ActionResult AdminMain()
        {
            //Stop Accessing the action if not logged in
            //or account not in the "Administrator" role
            if (HttpContext.Session.GetString("Role") == null ||
              (HttpContext.Session.GetString("Role") != "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult JudgeMain()
        {
            //Stop Accessing the action if not logged in
            //or account not in the "Judge" role
            if (HttpContext.Session.GetString("Role") == null ||
               (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult CompetitorMain()
        {
            //Stop Accessing the action if not logged in
            //or account not in the "Competitor" role
            if (HttpContext.Session.GetString("Role") == null ||
               (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();            
            // Call the Index action of Home controller
            return RedirectToAction("PublicMain");
        }

        public IActionResult Voting()
        {
            return View();
        }
        [Authorize]
        public async Task<ActionResult> StudentLogin()
        {
            // The user is already authenticated, so this call won't
            // trigger login, but it allows us to access token related values.
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            string idToken = auth.Properties.GetTokenValue(
             OpenIdConnectParameterNames.IdToken);
            try
            {
                // Verify the current user logging in with Google server
                // if the ID is invalid, an exception is thrown
                Payload currentUser = await
                GoogleJsonWebSignature.ValidateAsync(idToken);
                string userName = currentUser.Name;
                string eMail = currentUser.Email;
                HttpContext.Session.SetString("LoginID", userName + " / "
                + eMail);
                HttpContext.Session.SetString("Role", "Competitor");
                HttpContext.Session.SetString("LoggedInTime",
                 DateTime.Now.ToString());
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                // Token ID is may be tempered with, force user to logout
                return RedirectToAction("LogOut");
            }

        }
    }
}
