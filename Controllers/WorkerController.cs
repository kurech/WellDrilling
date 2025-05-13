using Microsoft.AspNetCore.Mvc;
using robert.Data;
using robert.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace robert.Controllers
{
    public class WorkerController(ApplicationDbContext db) : Controller
    {
        private readonly ApplicationDbContext db = db;
        public IActionResult Index(int workerid)
        {
            if (Request.Cookies["welldrilling"] == null)
            {
                return Redirect("/Access/Authorization");
            }

            var handler = new JwtSecurityTokenHandler();
            var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;
            var role = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "role").Value;

            if (role == "Оператор" || role == "Рабочий")
            {

                var model = new CommonViewModel
                {
                    ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
                };

                ViewBag.Worker = db.Users.SingleOrDefault(x => x.Id == workerid);
                return PartialView(model);
            }
            else
            {
                return Forbid();
            }
        }

        public IActionResult Schedule()
        {
            ViewBag.ActivePage = "Расписание";
            if (Request.Cookies["welldrilling"] == null)
            {
                return Redirect("/Access/Authorization");
            }

            var handler = new JwtSecurityTokenHandler();
            var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;
            var role = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "role").Value;

            if (role != "Рабочий")
            {
                return Forbid();
            }

            var model = new CommonViewModel
            {
                ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
            };

            var mywelluser = db.WellUsers.Where(x => x.UserId == int.Parse(userId));
            var myscheduales = mywelluser.Select(wu => wu.WorkSchedule).ToList();

            ViewBag.MyWorkSchedules = myscheduales.OrderByDescending(x => x.StartDate).ToList();
            ViewBag.WellUsers = db.WellUsers.ToList();
            ViewBag.Wells = db.Wells.ToList();

            return View(model);
        }

        public IActionResult Main()
        {
            ViewBag.ActivePage = "Сегодня";
            if (Request.Cookies["welldrilling"] == null)
            {
                return Redirect("/Access/Authorization");
            }

            var handler = new JwtSecurityTokenHandler();
            var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;
            var role = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "role").Value;

            if (role != "Рабочий")
            {
                return Forbid();
            }

            List<WorkSchedule> workSchedules = []; // Инициализация списка
            WorkSchedule? nowworkschedule = null; // Инициализация переменной

            var today = DateTime.Now.Date;

            foreach (var ws in db.WorkSchedules)
            {
                var startdate = ws.StartDate.Date;
                var enddate = ws.EndDate.Date;

                if (startdate <= today && today <= enddate) // Исправлено для включения даты startdate и enddate
                {
                    workSchedules.Add(ws);
                }
            }

            foreach (var wl in db.WellUsers)
            {
                foreach (var ws in workSchedules)
                {
                    if (wl.WorkScheduleId == ws.Id && wl.UserId == int.Parse(userId))
                    {
                        nowworkschedule = ws;

                        ViewBag.TodayWorkSchedule = nowworkschedule;
                        
                    }
                }
            }

            if (nowworkschedule == null)
            {
                ViewBag.TodayWorkSchedule = null;
            }
            else
            {
                WellUser welluser = db.WellUsers.Single(x => x.WorkScheduleId == nowworkschedule.Id && x.UserId == int.Parse(userId));
                ViewBag.Well = db.Wells.First(x => x.Id == welluser.WellId);
                ViewBag.ThisScheduleWellUser = db.WellUsers.Where(x => x.WorkScheduleId == nowworkschedule.Id).ToList();
            }

            var model = new CommonViewModel{ ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)) };
            return View(model);
        }
    }
}
