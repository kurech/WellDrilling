using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using robert.Data;
using robert.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace robert.Controllers
{
    public class OrderController(ApplicationDbContext db) : Controller
    {
        private readonly ApplicationDbContext db = db;
        public IActionResult Index(int? orderid)
        {
            if (Request.Cookies["welldrilling"] != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;

                if (db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)).Role == "Администратор")
                    return Forbid();
                if (db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)).Role == "Рабочий")
                    return Forbid();

                var model = new CommonViewModel
                {
                    ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
                };

                List<WorkOrder> workOrders = [.. db.WorkOrders.OrderByDescending(x => x.OrderDate)];

                if (orderid != null)
                {
                    workOrders = workOrders.Where(x => x.Id == orderid).ToList();
                    ViewBag.SearchedName = orderid;
                }

                ViewBag.Clients = db.Clients.ToList();
                ViewBag.Wells = db.Wells.ToList();
                ViewBag.WellUsers = db.WellUsers.ToList();
                ViewBag.Users = db.Users.ToList();
                ViewBag.WorkOrders = workOrders;

                ViewBag.ActivePage = "Заказы";
                return View(model);
            }
            return Redirect("/Access/Authorization");
        }

        public IActionResult Add()
        {
            ViewBag.Clients = db.Clients.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Wells = db.Wells.ToList();
            return PartialView();
        }

        public IActionResult Single(int orderid)
        {
            if (Request.Cookies["welldrilling"] == null)
                return Redirect("/Access/Authorization");

            var handler = new JwtSecurityTokenHandler();
            var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;

            if (handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "role").Value == "Рабочий")
                return Forbid();

            var model = new CommonViewModel
            {
                ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
            };

            var wo = db.WorkOrders.SingleOrDefault(wo => wo.Id == orderid);
            ViewBag.WorkOrder = wo;
            ViewBag.Client = db.Clients.Single(x => x.Id == wo.ClientId);
            ViewBag.Well = db.Wells.Single(x => x.Id == wo.WellId);
            ViewBag.WellUsers = db.WellUsers.Where(x => x.WorkScheduleId == wo.WorkScheduleId).ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.WorkSchedule = db.WorkSchedules.SingleOrDefault(x => x.Id == wo.WorkScheduleId);
            return PartialView(model);
        }
    }
}
