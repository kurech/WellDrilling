using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using robert.Data;
using robert.Models;
using System.IdentityModel.Tokens.Jwt;

namespace robert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkSchedulesController(ApplicationDbContext db) : Controller
    {
        private readonly ApplicationDbContext db = db;

        [HttpGet("GetPlannedCost/{workorderid}")]
        public JsonResult GetPlannedCost(int workorderid)
        {
            var workorder = db.WorkOrders.SingleOrDefault(x => x.Id == workorderid);
            if (workorder != null)
            {
                return new JsonResult(workorder.Cost);
            }
            return new JsonResult(NotFound());
        }

        [HttpPost("Add/{workorderid}/{startdate}/{enddate}/{plannedcost}")]
        public JsonResult Add(int workorderid, DateTime startdate, DateTime enddate, float plannedcost, string[] users)
        {
            var workorder = db.WorkOrders.Single(x => x.Id == workorderid);

            string description = workorder.Description;

            WorkSchedule workschedule = new()
            {
                Description = description,
                StartDate = startdate,
                EndDate = enddate,
                PlannedCost = plannedcost,
                Status = "Создан"
            };
            db.WorkSchedules.Add(workschedule);
            db.SaveChanges();

            for (int i = 0; i < users.Length; i++)
            {
                WellUser welluser = new()
                {
                    WellId = workorder.WellId,
                    UserId = int.Parse(users[i]),
                    WorkScheduleId = workschedule.Id,
                    IsReady = false
                };
                db.WellUsers.Add(welluser);
                db.SaveChanges();
            }

            workorder.WorkScheduleId = workschedule.Id;
            db.WorkOrders.Update(workorder);
            db.SaveChanges();

            return new JsonResult(Ok());
        }

        [HttpPut("SetReadyByWorker/{welluserid}")]
        public JsonResult SetReadyByWorker(int welluserid)
        {
            var welluser = db.WellUsers.SingleOrDefault(s => s.Id == welluserid);

            if (welluser == null)
                return new JsonResult(NotFound());

            welluser.IsReady = true;
            db.WellUsers.Update(welluser);
            db.SaveChanges();
            return new JsonResult(Ok());
        }

        [HttpPut("SetStatus/{woid}/{variable}")]
        public JsonResult SetStatus(int woid, string variable)
        {
            var schedule = db.WorkSchedules.SingleOrDefault(x => x.Id == woid);
            if (schedule == null)
                return new JsonResult(NotFound());

            schedule.Status = variable;
            db.WorkSchedules.Update(schedule);
            db.SaveChanges();
            return new JsonResult(Ok());
        }

        [HttpPost("SendNotificationToWorker/{workscheduleid}")]
        public JsonResult SendNotificationToWorker(int workscheduleid)
        {
            var handler = new JwtSecurityTokenHandler();
            var userId = handler.ReadJwtToken(Request.Cookies["welldrilling"]).Claims.First(c => c.Type == "userId").Value;
            var operuser = db.Users.SingleOrDefault(m => m.Id == int.Parse(userId));

            var unreadywellusers = db.WellUsers.Where(x => x.WorkScheduleId == workscheduleid && x.IsReady == false).ToList();
            var well = db.Wells.First(x => x.Id == unreadywellusers.ElementAt(0).WellId);

            foreach (var wu in unreadywellusers)
            {
                var user = db.Users.First(x => x.Id == wu.UserId);
                Notification notification = new()
                {
                    UserId = user.Id,
                    User = user,
                    NotificationType = "Просрочен дедлайн",
                    DateTime = DateTime.Now,
                    Content = $"Просрочен дедлайн выполнения работы на скважине {well.Name}. Координаты скважины: {well.Latitude};{well.Longitude}. Поставьте отметку о выполнении или свяжитесь с оператором {operuser.Username}."
                };

                db.Notifications.Add(notification);
                db.SaveChanges();
            }

            return new JsonResult(Ok());
        }

        [HttpGet("CheckAvailableWorker/{workerid}/{startdate}/{enddate}")]
        public JsonResult CheckAvailableWorker(int workerid, DateTime startdate, DateTime enddate)
        {
            var wuIds = db.WellUsers.Where(x => x.UserId == workerid).Select(x => x.WorkScheduleId).ToList();

            var result = db.WorkSchedules
                            .Where(ws => wuIds.Contains(ws.Id) &&
                                         ((ws.StartDate >= startdate && ws.StartDate <= enddate) ||
                                          (ws.EndDate >= startdate && ws.EndDate <= enddate) ||
                                          (ws.StartDate <= startdate && ws.EndDate >= enddate)))
                            .Any();

            return new JsonResult(Ok(result));
        }
    }
}
