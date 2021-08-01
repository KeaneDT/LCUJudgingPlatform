using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WEB_Assignment_Team4.Models;

namespace WEB_Assignment_Team4.Controllers
{
    public class HolidayController : Controller
    {
        // GET: HolidayController
        public async Task<ActionResult> Index()
        {
            HttpClient client = new HttpClient();
            string year = DateTime.Today.Year.ToString();
            string month = DateTime.Today.Month.ToString();
            string day = DateTime.Today.Day.ToString();
            
            client.BaseAddress = new Uri("https://holidays.abstractapi.com/v1/?api_key=bd9022d26baf47dd8ddba77582ba3e24&country=SG&year="+year+"&month="+"1"+"&day="+day);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<Holiday> holList = JsonConvert.DeserializeObject<List<Holiday>>(data);
                return View(holList);
            }
            else
            {
                return View(new List<Holiday>());
            }
        }

        // GET: HolidayController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HolidayController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HolidayController/Create
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

        // GET: HolidayController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HolidayController/Edit/5
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

        // GET: HolidayController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HolidayController/Delete/5
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
