using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using robert.Data;
using robert.Models;

namespace robert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrdersController(ApplicationDbContext db) : ControllerBase
    {
        private readonly ApplicationDbContext db = db;

        [HttpPost("Add/{client}/{well}/{orderdate}/{description}/{cost}")]
        public JsonResult Add(int client, int well, DateTime orderdate, string description, float cost)
        {
            WorkOrder wo = new()
            {
                ClientId = client,
                WellId = well,
                OrderDate = orderdate,
                Description = description,
                Cost = cost
            };

            db.WorkOrders.Add(wo);
            db.SaveChanges();

            return new JsonResult(wo);
        }
    }
}
