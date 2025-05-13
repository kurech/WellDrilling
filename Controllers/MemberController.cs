using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using robert.Data;
using robert.Models;
using System.IdentityModel.Tokens.Jwt;

namespace robert.Controllers
{
    public class MemberController(ApplicationDbContext db) : Controller
    {
        private readonly ApplicationDbContext db = db;

        public IActionResult Index(string? query)
        {
            if (Request.Cookies["welldrilling"] != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;

                if (db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)).Role != "Администратор")
                    return Forbid();

                var model = new CommonViewModel
                {
                    ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
                };

                var users = db.Users.Where(x => x.Role != "Администратор").ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    users = users.Where(x => (x.LastName + " " + x.FirstName).ToLower().Contains(query.ToLower())).ToList();
                    ViewBag.SearchedName = query;
                }

                ViewBag.Users = users;
                ViewBag.ActivePage = "Пользователи";
                return View(model);
            }
            return Redirect("/Access/Authorization");
        }

        public IActionResult Edit(int userid)
        {
            ViewBag.User = db.Users.SingleOrDefault(x => x.Id == userid);
            return PartialView();
        }

        public IActionResult Add() => PartialView();
    }
}
