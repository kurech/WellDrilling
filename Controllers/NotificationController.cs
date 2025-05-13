using Microsoft.AspNetCore.Mvc;
using robert.Data;
using System.IdentityModel.Tokens.Jwt;

namespace robert.Controllers
{
    public class NotificationController(ApplicationDbContext _db) : Controller
    {
        private readonly ApplicationDbContext db = _db;
        public IActionResult Index()
        {
            ViewBag.ActivePage = "Уведомления";
            if (Request.Cookies["welldrilling"] != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;

                if (db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)).Role != "Рабочий")
                    return Forbid();

                var model = new CommonViewModel
                {
                    ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
                };

                ViewBag.Notifications = db.Notifications.Where(x => x.UserId == model.ThisUser.Id).OrderByDescending(x => x.DateTime).ToList();

                return View(model);
            }
            return Redirect("/Access/Authorization");
        }

        public IActionResult Display(int notificationid)
        {
            var notification = db.Notifications.Single(x => x.Id == notificationid);
            ViewBag.Notification = notification;

            notification.IsRead = true;
            db.Notifications.Update(notification);
            db.SaveChanges();

            return PartialView();
        }
    }
}
