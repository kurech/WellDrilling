using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using robert.Data;
using robert.Models;
using System.Net;

namespace robert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public ClientsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpPost("Add/{lastname}/{firstname}/{contactnumber}/{email}/{address}/{clienttype}")]
        public JsonResult Add(string lastname, string firstname, string contactnumber, string email, string address, int clienttype)
        {
            Client client = new()
            {
                FirstName = firstname,
                LastName = lastname,
                ContactNumber = contactnumber,
                Email = email,
                Address = address,
                ClientType = (ClientType)clienttype,
                RegistrationDate = DateTime.Now,
                IsDeleted = false
            };

            db.Clients.Add(client);
            db.SaveChanges();

            return new JsonResult(client);
        }

        [HttpPut("Edit/{clientid}/{lastname}/{firstname}/{contactnumber}/{email}/{address}/{clienttype}")]
        public JsonResult Edit (int clientid, string lastname, string firstname, string contactnumber, string email, string address, int clienttype)
        {
            var client = db.Clients.SingleOrDefault(client => client.Id == clientid);
            if (client == null)
                return new JsonResult(NotFound());
            
            client.FirstName = firstname;
            client.LastName = lastname;
            client.ContactNumber = contactnumber;
            client.Email = email;
            client.Address = address;
            client.ClientType = ActionsManager.ConvertFromIndex(clienttype);

            db.Clients.Update(client);
            db.SaveChanges();

            return new JsonResult(client);
        }

        [HttpPut("IsDeletedTrue/{clientid}")]
        public JsonResult IsDeletedTrue (int clientid)
        {
            var client = db.Clients.SingleOrDefault(client => client.Id == clientid);

            if (client == null)
                return new JsonResult(NotFound());

            client.IsDeleted = true;
            db.Clients.Update(client);
            db.SaveChanges();

            return new JsonResult(Ok(client));
        }

        [HttpPut("IsDeletedFalse/{clientid}")]
        public JsonResult IsDeletedFalse (int clientid)
        {
            var client = db.Clients.SingleOrDefault(client => client.Id == clientid);

            if (client == null)
                return new JsonResult(NotFound());

            client.IsDeleted = false;
            db.Clients.Update(client);
            db.SaveChanges();

            return new JsonResult(Ok(client));
        }
    }
}
