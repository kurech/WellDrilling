using Microsoft.AspNetCore.Mvc;
using robert.Data;
using System.IdentityModel.Tokens.Jwt;

namespace robert.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult Authorization()
        {
            if (Request.Cookies["welldrilling"] != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;
                var role = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "role").Value;

                if (role == "Рабочий")
                    return Redirect("/Worker/Main");
                else if (role == "Оператор")
                    return Redirect("/Schedule/List");
                else if (role == "Менеджер")
                    return Redirect("/Customer/Index");
                else if (role == "Администратор")
                    return Redirect("/Member/Index");
            }
            return View();
        }
        public IActionResult Registration() => View();
    }
}
