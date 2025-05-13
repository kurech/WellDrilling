using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using robert.Data;
using robert.Models;
using System.Net.NetworkInformation;

namespace robert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellsController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public WellsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpPost("Add/{name}/{latitude}/{longitude}/{depth}/{diameter}/{drillingmethod}/{soiltype}")]
        public JsonResult Add(string name, double latitude, double longitude, double depth, double diameter, string drillingmethod, string soiltype)
        {
            Well well = new()
            {
                Name = name,
                Latitude = latitude,
                Longitude = longitude,
                Depth = depth,
                Diameter = diameter,
                DrillingMethod = drillingmethod,
                SoilType = soiltype
            };

            db.Wells.Add(well);
            db.SaveChanges();

            return new JsonResult(Ok(well));
        }

        [HttpGet("All")]
        public List<Well> All() => [.. db.Wells];

        [HttpPost("Edit/{id}/{name}/{latitude}/{longitude}/{depth}/{diameter}/{drillingmethod}/{soiltype}")]
        public JsonResult Edit(int id, string name, double latitude, double longitude, double depth, double diameter, string drillingmethod, string soiltype)
        {
            var well = db.Wells.SingleOrDefault(w => w.Id == id);

            if (well != null)
            {
                well.Name = name;
                well.Latitude = latitude;
                well.Longitude = longitude;
                well.Depth = depth;
                well.Diameter = diameter;
                well.DrillingMethod = drillingmethod;
                well.SoilType = soiltype;

                db.Wells.Update(well);
                db.SaveChanges();

                return new JsonResult(well);
            }
            return new JsonResult(NotFound());
        }
    }
}
