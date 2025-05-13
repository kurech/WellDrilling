using robert.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace robert.Data
{
    public class ActionsManager
    {
        public static string Hashing(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = SHA256.HashData(inputBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hash;
        }

        public static ClientType ConvertFromIndex(int clienttype)
        {
            switch (clienttype) 
            { 
                case 0:
                    return ClientType.Физическое;
                case 1:
                    return ClientType.Юридическое;
                default:
                    return ClientType.Физическое;
            }
        }

        public static double GetAverageSalary(ApplicationDbContext db, int workerId)
        {
            var schedulesids = db.WellUsers.Where(x => x.UserId == workerId).Select(x => x.WorkScheduleId).ToList();
            var scheduleslist = new List<WorkSchedule>();
            foreach (var a in schedulesids)
            {
                foreach (var b in db.WorkSchedules.ToList())
                {
                    if (a == b.Id)
                    {
                        scheduleslist.Add(b);
                    }
                }
            }

            List<double> salaries = new();
            foreach (var a in scheduleslist.Where(x => x.Status == "Выполнено"))
            {
                var commoncost = a.PlannedCost;

                var countpeople = db.WellUsers.Count(x => x.WorkScheduleId == a.Id);

                salaries.Add(commoncost / countpeople);
            }

            double sum = 0;
            foreach (var salary in salaries)
            {
                sum += salary;
            }

            return sum / salaries.Count;
        }

        public static double GetWorkedDays(ApplicationDbContext db, int workerId)
        {
            var schedulesids = db.WellUsers.Where(x => x.UserId == workerId).Select(x => x.WorkScheduleId).ToList();
            var scheduleslist = new List<WorkSchedule>();
            foreach (var a in schedulesids)
            {
                foreach (var b in db.WorkSchedules.ToList())
                {
                    if (a == b.Id)
                    {
                        scheduleslist.Add(b);
                    }
                }
            }

            double dayscount = 0;
            foreach (var a in scheduleslist.Where(x => x.Status == "Выполнено"))
            {
                TimeSpan thisdays = a.EndDate - a.StartDate;
                dayscount += thisdays.TotalDays;
            }

            return dayscount;
        }

        public async static void SendMessageToEmail(string email, string subject, string body)
        {
            MailAddress from = new MailAddress("ranelgilyazov003@gmail.com", "WellDrilling");
            MailAddress to = new MailAddress(email);

            string htmlBody = $@"
            <div style='font-family: Arial, Helvetica, sans-serif;'>
                <div style='text-align:center;'>
                    <h1>Регистрация в системе WellDrilling</h1>
                </div>
                <div>
                    <p style='font-size: 16px; color: #000000 !important'>
                        {body}
                    </p>
                </div>
                <div style='text-align: center; background-color: #0d6efd; padding: 16px 32px;'>
                    <p style='color: white; font-size: 20px;'>2024 &#8212; WellDrilling</pa>
                </div>
            </div>";

            MailMessage message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            SmtpClient smtp = new SmtpClient("smtp.elasticemail.com", 2525);
            smtp.Credentials = new NetworkCredential("welldrilling@well.drilling", "54EE46A87E40C40535592645A6322C2CD56A");

            await smtp.SendMailAsync(message);
        }
    }
}
