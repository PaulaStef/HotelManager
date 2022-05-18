using Aspose.Pdf;
using BusinessLayer.Contracts.Interfaces;
using BusinessLayer.Contracts.Models;
using DataAccess.Contracts;
using DataAccess.Contracts.Entity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using BusinessLayer.Contracts.Factory;

namespace BusinessLayer.Contracts.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentFactory documentFactory;

        public InvoiceService(IUnitOfWork unitOfWork, IDocumentFactory documentFactory)
        {
            _unitOfWork = unitOfWork;
            this.documentFactory = documentFactory;
        }
        public string AddInvoice(InvoiceModel invoice)
        {
            var invoice2 = Model2Entity(invoice);
            if (invoice2 == null)
            {
                return "Room not found";
            }
            else
            {
                _unitOfWork._invoiceRepository.Add(invoice2);
                _unitOfWork.SaveChanges();
                string name = createPDF(invoice2);
                sendMail(invoice2.Message, name);
                return "Succes";
            }
        }

        public InvoiceEntity Model2Entity(InvoiceModel invoice)
        {
            var reservation = _unitOfWork._reservationRepository.GetAllQueryable().Where(x=> x.Id == invoice.ReservationId).Include(x => x.Rooms).FirstOrDefault();
            InvoiceEntity Newinvoice = null;
            double total = 0;
            if (reservation != null)
            {
                foreach (var room in reservation.Rooms)
                {
                    total += room.Price;
                }
                if (reservation.Reduction != 0)
                {
                    total = reservation.Reduction / 100.0;
                }
                double days = (reservation.CheckOut - reservation.CheckIn).TotalDays;
                System.Diagnostics.Debug.WriteLine("days = " + days);
                total *= days;
                Newinvoice = new InvoiceEntity
                {
                    Message = invoice.Message,
                    TotalPrice = total,
                    Reservation = reservation
                };
            }
            return Newinvoice;

        }
        string createPDF(InvoiceEntity invoice)
        {
            Document document = new Document();
            Page page = document.Pages.Add();

            int number = _unitOfWork._invoiceRepository.GetAll().Count();
            StringBuilder sb = new StringBuilder("          Invoice number ",200);
            sb.AppendFormat("{0}\n\n\n", number);
            sb.AppendLine("Name: " + invoice.Reservation.ClientName + "\n");
            sb.AppendLine("Mail Address: " + invoice.Reservation.ClientEmail + "\n");
            sb.AppendLine("Check In Date: " + invoice.Reservation.CheckIn + "\n");
            sb.AppendLine("Check Out Date: " + invoice.Reservation.CheckOut + "\n");
            sb.AppendLine("Number of Rooms: " + invoice.Reservation.Rooms.Count() + "\n");
            sb.AppendLine("Reduction for loyality: " + invoice.Reservation.Reduction + "\n");
            sb.AppendLine("Total price: " + invoice.TotalPrice);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(sb.ToString()));
            string name = "Invoice" + number + ".pdf";
            document.Save(name);
            return name;
        }

        void sendMail(string message, string pdf)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("contps406@gmail.com", "P@ssw0rd122"),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("contps406@gmail.com"),
                Subject = "Invoice Hotel",
                Body = message
            };
            mailMessage.To.Add("stefpaula00@gmail.com");
            var attachment = new Attachment(pdf);
            mailMessage.Attachments.Add(attachment);
            smtpClient.Send(mailMessage);
           
        }

        public static InvoiceModel Entity2Model(InvoiceEntity reservation)
        {
            return new InvoiceModel
            {
                ReservationId = reservation.Id,
                Message = reservation.Message,
                TotalPrice = reservation.TotalPrice 
            };
        }

        public IActionResult getThisMonthInvoices(string type)
        {
            List<InvoiceModel> invoices = new List<InvoiceModel>();
           DateTime dt = DateTime.Now;
           var all = _unitOfWork._invoiceRepository.GetAllQueryable().Include(x => x.Reservation).Where(x => x.Reservation.CheckOut.Month == dt.Month).ToList();
            foreach (var invoice in all)
            {
                if (invoice != null)
                {
                    invoices.Add(Entity2Model(invoice));
                }
            }
            return documentFactory.CreateExport(type).Export<InvoiceModel>(invoices);
        }
    }
}
