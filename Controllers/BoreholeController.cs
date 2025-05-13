using Microsoft.AspNetCore.Mvc;
using robert.Data;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace robert.Controllers
{
    public class BoreholeController : Controller
    {
        private readonly ApplicationDbContext db;
        public BoreholeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index(string? wellname)
        {
            if (Request.Cookies["welldrilling"] != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;

                if (db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)).Role != "Менеджер")
                    return Forbid();

                var model = new CommonViewModel
                {
                    ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
                };

                var wells = db.Wells.ToList();

                if (!string.IsNullOrEmpty(wellname))
                {
                    wells = wells.Where(w => w.Name.ToLower().Contains(wellname.ToLower())).ToList();
                    ViewBag.SearchedName = wellname;
                }

                ViewBag.Wells = wells;
                ViewBag.ActivePage = "Скважины";
                return View(model);
            }
            return Redirect("/Access/Authorization");
        }

        public IActionResult Add() => PartialView();

        public IActionResult Display(int wellid)
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
                ViewBag.Well = db.Wells.SingleOrDefault(x => x.Id == wellid);
                return PartialView(model);
            }
            return Redirect("/Access/Authorization");
            
        }
    }
}
