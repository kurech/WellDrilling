using Microsoft.AspNetCore.Mvc;
using robert.Data;
using System.IdentityModel.Tokens.Jwt;

namespace robert.Controllers
{
    public class ProfileController(ApplicationDbContext db) : Controller
    {
        private readonly ApplicationDbContext db = db;

        public IActionResult Index()
        {
            ViewBag.ActivePage = "Профиль";
            if (Request.Cookies["welldrilling"] != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;

                if (db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)).Role == "Администратор")
                    return Forbid();

                var model = new CommonViewModel
                {
                    ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
                };

                return View(model);
            }
            return Redirect("/Access/Authorization");
        }
    }
}
