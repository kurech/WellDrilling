using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using robert.Data;
using robert.Models;
using System.IdentityModel.Tokens.Jwt;

namespace robert.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext db;
        public CustomerController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index(string? query)
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

                List<Client> clients = [.. db.Clients.OrderByDescending(x => x.RegistrationDate)];

                if (!string.IsNullOrEmpty(query))
                {
                    clients = clients.Where(x => (x.LastName + " " + x.FirstName).Contains(query, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    ViewBag.SearchedName = query;
                }

                ViewBag.Clients = clients;
                ViewBag.ActivePage = "Клиенты";

                return View(model);
            }
            return Redirect("/Access/Authorization");
        }
        
        public IActionResult Add() => PartialView();

        public IActionResult Display(int customerid)
        {
            var handler = new JwtSecurityTokenHandler();
            var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;

            var customer = db.Clients.SingleOrDefault(customer => customer.Id == customerid);
            ViewBag.Client = customer;

            var model = new CommonViewModel
            {
                ThisUser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId)),
            };
            return PartialView(model);
        }
    }
}
