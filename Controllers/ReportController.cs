using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using robert.Data;
using robert.Models;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace robert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController(ApplicationDbContext db) : ControllerBase
    {
        ApplicationDbContext db = db;

        [HttpGet("WorkerAll/{workerid}")]
        public ActionResult WorkerAll (int workerid)
        {
            var user = db.Users.First(x => x.Id == workerid);

            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 15, 15, 15, 15);
                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                document.Open();

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1252");

                BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\ARIAL.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font headerfont = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

                Paragraph paragraph = new Paragraph($"Отчет по выполненой работе от {DateTime.Now.ToString("f")}", headerfont);
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.Font.Size = 14;
                paragraph.Font.IsBold();
                paragraph.SpacingAfter = 6f;
                document.Add(paragraph);

                Paragraph username = new Paragraph($"{user.LastName} {user.FirstName}", headerfont);
                username.Alignment = Element.ALIGN_CENTER;
                username.Font.Size = 14;
                username.SpacingAfter = 10f;
                document.Add(username);

                Paragraph avgsalary = new Paragraph($"Средняя зарплата {ActionsManager.GetAverageSalary(db, workerid)} рублей за одну скважину.", headerfont);
                avgsalary.Alignment = Element.ALIGN_LEFT;
                avgsalary.Font.Size = 12;
                avgsalary.SpacingAfter = 10f;
                document.Add(avgsalary);

                Paragraph workeddays = new Paragraph($"Проработано {ActionsManager.GetWorkedDays(db, workerid)} дней.", headerfont);
                workeddays.Alignment = Element.ALIGN_LEFT;
                workeddays.Font.Size = 12;
                workeddays.SpacingAfter = 10f;
                document.Add(workeddays);

                var wellslist = db.WellUsers
                    .Where(x => x.UserId == workerid)
                    .ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Работал на скважинах ({wellslist.Count()}):");
                int i = 0;
                foreach (var w in wellslist)
                {
                    i++;
                    var well = db.Wells.FirstOrDefault(x => x.Id == w.WellId);
                    var workshedule = db.WorkSchedules.FirstOrDefault(x => x.Id == w.WorkScheduleId);
                    string s = i.ToString() + ". " + well.Name + ": " + workshedule.Description + ".";
                    sb.AppendLine(s);
                }

                Paragraph weells = new Paragraph(sb.ToString(), headerfont);
                weells.Alignment = Element.ALIGN_LEFT;
                weells.Font.Size = 12;
                weells.SpacingAfter = 10f;
                document.Add(weells);

                Paragraph watermark = new Paragraph("Well Drilling", headerfont);
                watermark.Alignment = Element.ALIGN_RIGHT;
                watermark.Font.Size = 12;
                watermark.SpacingAfter = 10f;
                document.Add(watermark);

                document.Close();

                return new FileContentResult(stream.GetBuffer(), "application/pdf")
                {
                    FileDownloadName = $"Отчет по выполненой работе от {DateTime.Now.ToString("f")}.pdf"
                };
            }
        }

        [HttpGet("Order/{woid}")]
        public ActionResult Order(int woid)
        {
            var order = db.WorkOrders.Single(x => x.Id == woid);
            var client = db.Clients.Single(x => x.Id == order.ClientId);
            var well = db.Wells.Single(x => x.Id == order.WellId);
            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 15, 15, 15, 15);
                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                document.Open();

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1252");

                BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\ARIAL.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font headerfont = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

                Paragraph paragraph = new Paragraph($"Отчет по заказу №{order.Id} от {DateTime.Now.ToString("f")}", headerfont);
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.Font.Size = 14;
                paragraph.Font.IsBold();
                paragraph.SpacingAfter = 6f;
                document.Add(paragraph);

                Paragraph orderp = new Paragraph($"Заказ предназначен для клиента {client.LastName} {client.FirstName}, контактный номер телефона - {client.ContactNumber}.", headerfont);
                orderp.Alignment = Element.ALIGN_LEFT;
                orderp.Font.Size = 12;
                orderp.SpacingAfter = 10f;
                document.Add(orderp);

                Paragraph wellp = new Paragraph($"Бурение скважины {well.Name} глубиной {well.Depth} и шириной {well.Diameter}. Координаты скважины - {well.Latitude}; {well.Longitude}.", headerfont);
                wellp.Alignment = Element.ALIGN_LEFT;
                wellp.Font.Size = 12;
                wellp.SpacingAfter = 10f;
                document.Add(wellp);

                Paragraph descp = new Paragraph($"Описание работы - {order.Description}", headerfont);
                descp.Alignment = Element.ALIGN_LEFT;
                descp.Font.Size = 12;
                descp.SpacingAfter = 10f;
                document.Add(descp);

                var wsstr = string.Empty;
                if (order.WorkScheduleId != null)
                    wsstr = "Рабочие на заказ назначены.";
                else
                    wsstr = "Рабочих еще не назначили на заказ.";
                Paragraph workerp = new Paragraph(wsstr, headerfont);
                workerp.Alignment = Element.ALIGN_LEFT;
                workerp.Font.Size = 12;
                workerp.SpacingAfter = 10f;
                document.Add(workerp);

                string status = string.Empty;
                if (order.WorkScheduleId != null)
                {
                    status = "Статус выполнения работ: ";
                    status += db.WorkSchedules.Single(x => x.Id == order.WorkScheduleId).Status.ToLower();
                    status += ".";
                    Paragraph statusp = new Paragraph(status, headerfont);
                    statusp.Alignment = Element.ALIGN_LEFT;
                    statusp.Font.Size = 12;
                    statusp.SpacingAfter = 10f;
                    document.Add(statusp);
                }

                string dates = string.Empty;
                if (order.WorkScheduleId != null)
                {
                    dates = "Даты работы на этом заказы: ";
                    dates += db.WorkSchedules.Single(x => x.Id == order.WorkScheduleId).StartDate.ToShortDateString() + " - ";
                    dates += db.WorkSchedules.Single(x => x.Id == order.WorkScheduleId).EndDate.ToShortDateString();
                    dates += ".";

                    Paragraph datesp = new Paragraph(dates, headerfont);
                    datesp.Alignment = Element.ALIGN_LEFT;
                    datesp.Font.Size = 12;
                    datesp.SpacingAfter = 10f;
                    document.Add(datesp);
                }

                Paragraph watermark = new Paragraph("Well Drilling", headerfont);
                watermark.Alignment = Element.ALIGN_RIGHT;
                watermark.Font.Size = 12;
                watermark.SpacingAfter = 10f;
                document.Add(watermark);

                document.Close();

                return new FileContentResult(stream.GetBuffer(), "application/pdf")
                {
                    FileDownloadName = $"Отчет по заказу №{order.Id} от {DateTime.Now.ToString("f")}.pdf"
                };
            }
        }
    }
}
