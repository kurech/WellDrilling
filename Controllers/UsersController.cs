using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using robert.Data;
using robert.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace robert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly IConfiguration configuration;

        public UsersController(ApplicationDbContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }

        [HttpPost("Add/{email}/{password}/{firstname}/{lastname}/{role}")]
        public JsonResult Add(string email, string password, string firstname, string lastname, string role)
        {
            var emailavailable = db.Users.FirstOrDefault(x => x.Username == email);
            if (emailavailable != null)
            {
                return new JsonResult(NotFound());
            }

            User user = new()
            {
                Username = email,
                Password = ActionsManager.Hashing(password),
                Role = role,
                FirstName = firstname,
                LastName = lastname,
                RegistrationDate = DateTime.Now,
                IsBlocked = false
            };

            db.Users.Add(user);
            db.SaveChanges();

            ActionsManager.SendMessageToEmail(email, "Регистрация", $"<p>Здравствуйте, вас зарегистрировали в системе WellDrilling</p><br><p>Логин: <b>{email}</b></p><p>Пароль: <b>{password}</b></p>");

            return new JsonResult(Ok(user));
        }

        [HttpGet("Login/{email}/{password}")]
        public JsonResult Login(string email, string password)
        {
            var user = db.Users.SingleOrDefault(user => user.Username == email && user.Password == ActionsManager.Hashing(password) && user.IsBlocked == false);
            if (user == null)
            {
                return new JsonResult(NotFound());
            }
            else
            {
                var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("userId", user.Id.ToString()),
                        new Claim("email", user.Username),
                        new Claim("role", user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(jwtConfig.ExpirationInMinutes),
                    NotBefore = DateTime.UtcNow,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                Response.Cookies.Append(jwtConfig.CookieName, tokenHandler.WriteToken(token), new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddMinutes(jwtConfig.ExpirationInMinutes)
                });

                return new JsonResult(Ok(user));
            }
        }

        [HttpPut("Edit/{userid}/{role}/{lastname}/{firstname}")]
        public JsonResult Edit(int userid, string role, string lastname, string firstname)
        {
            var user = db.Users.SingleOrDefault(x => x.Id == userid);

            if (user != null)
            {
                user.Role = role;
                user.LastName = lastname;
                user.FirstName = firstname;

                db.Users.Update(user);
                db.SaveChanges();
            }

            return new JsonResult(user);
        }

        [HttpPost("Logout")]
        public JsonResult Logout()
        {
            Response.Cookies.Delete("welldrilling");
            return new JsonResult(Ok());
        }

        [HttpPut("EditInProfile/{userid}/{lastname}/{firstname}")]
        public JsonResult EditInProfile(int userid, string lastname, string firstname)
        {
            var user = db.Users.SingleOrDefault(y => y.Id == userid);
            if (user != null)
            {
                user.LastName = lastname;
                user.FirstName = firstname;

                db.Users.Update(user);
                db.SaveChanges();
            }

            return new JsonResult(user);
        }

        [HttpGet("CheckAvailableOldPassword/{userid}/{oldpassword}")]
        public JsonResult CheckAvailableOldPassword(int userid, string oldpassword)
        {
            var user = db.Users.SingleOrDefault(y => y.Id == userid && y.Password == ActionsManager.Hashing(oldpassword));
            if (user != null)
            {
                return new JsonResult(Ok(user));
            }
            return new JsonResult(NotFound());
        }

        [HttpPut("EditPassword/{userid}/{newpassword}")]
        public JsonResult EditPassword(int userid, string newpassword)
        {
            var user = db.Users.SingleOrDefault(y => y.Id == userid);
            if (user != null)
            {
                user.Password = ActionsManager.Hashing(newpassword);
                db.Users.Update(user);
                db.SaveChanges();
                return new JsonResult(Ok(user));
            }
            return new JsonResult(NotFound());
        }

        [HttpPut("Block/{userid}")]
        public JsonResult Block(int userid)
        {
            var user = db.Users.SingleOrDefault(y => y.Id == userid);
            if (user != null)
            {
                user.IsBlocked = true;
                db.Users.Update(user);
                db.SaveChanges();
                return new JsonResult(Ok(user));
            }
            return new JsonResult(NotFound());
        }

        [HttpPut("Unblock/{userid}")]
        public JsonResult Unblock(int userid)
        {
            var user = db.Users.SingleOrDefault(y => y.Id == userid);
            if (user != null)
            {
                user.IsBlocked = false;
                db.Users.Update(user);
                db.SaveChanges();
                return new JsonResult(Ok(user));
            }
            return new JsonResult(NotFound());
        }

        [HttpGet("GetUserById/{userid}")]
        public JsonResult GetUserById(int userid)
        {
            var user = db.Users.SingleOrDefault(y => y.Id == userid);
            if (user != null)
                return new JsonResult(Ok(user));
            return new JsonResult(NotFound());
        }
    }
}
