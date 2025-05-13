using Microsoft.AspNetCore.Mvc;
using robert.Data;
using robert.Models;
using System.IdentityModel.Tokens.Jwt;

namespace robert.Controllers
{
    public class ScheduleController(ApplicationDbContext db) : Controller
    {
        private readonly ApplicationDbContext db = db;

        public IActionResult Index()
        {
            if (Request.Cookies["welldrilling"] != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;

                if (db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)).Role != "Оператор")
                    return Forbid();

                var model = new CommonViewModel
                {
                    ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
                };

                ViewBag.WorkOrders = db.WorkOrders.Where(x => x.WorkScheduleId == null).ToList();
                ViewBag.Users = db.Users.Where(x => x.Role == "Рабочий").ToList();
                ViewBag.AllClients = db.Clients.ToList();
                ViewBag.AllWell = db.Wells.ToList();
                return View(model);
            }
            return Redirect("/Access/Authorization");
        }

        public IActionResult List()
        {
            if (Request.Cookies["welldrilling"] == null)
            {
                return Redirect("/Access/Authorization");
            }

            var handler = new JwtSecurityTokenHandler();
            var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;

            if (db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)).Role != "Оператор")
                return Forbid();

            var model = new CommonViewModel
            {
                ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId))
            };

            ViewBag.WorkSchedules = db.WorkSchedules.ToList();
            ViewBag.WellUsers = db.WellUsers.ToList();
            ViewBag.WorkOrders = db.WorkOrders.ToList();
            ViewBag.Users = db.Users.ToList();

            ViewBag.ActivePage = "Расписание";
            return View(model);
        }

        public IActionResult Display(int workscheduleid)
        {
            var workschedule = db.WorkSchedules.SingleOrDefault(s => s.Id == workscheduleid);

            if (workschedule == null)
                return NotFound();

            ViewBag.WorkSchedule = workschedule;
            ViewBag.WorkOrder = db.WorkOrders.First(x => x.WorkScheduleId == workscheduleid);
            var well = db.Wells.First(x => x.Id == db.WorkOrders.First(x => x.WorkScheduleId == workscheduleid).WellId);
            ViewBag.Well = well;
            ViewBag.WellUsers = db.WellUsers.Where(x => x.WorkScheduleId == workschedule.Id).ToList();
            ViewBag.Users = db.Users.Where(x => x.Role == "Рабочий").ToList();

            var handler = new JwtSecurityTokenHandler();
            var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;
            var model = new CommonViewModel{ ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId))};

            return PartialView(model);
        }
    }
}
