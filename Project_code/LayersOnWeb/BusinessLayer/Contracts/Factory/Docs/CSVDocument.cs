using BusinessLayer.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Contracts.Factory
{
    public class CSVDocument : IDocument
    {
        public IActionResult Export<T>(List<T> entities) where T : class
        {
            string data = "";
            data += "ClientName," +
                   "ClientEmail," +
                   "Reduction," +
                   "CheckIn + ," +
                   "CheckOut + ," +
                   "RoomsNo";
            data += "\n";
            foreach (var reservation in entities)
            {
                data += reservation.ToString();
                data += "\n";
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            var file = new FileContentResult(bytes, "application/csv_tickets")
            {
                FileDownloadName = "Tickets.csv"
            };

            return file;
        }

        public IActionResult Export(List<string> clients)
        {
            string data = "";
            data += "ClientEmail";
            data += "\n";
            foreach (string client in clients)
            {
                data += client;
                data += "\n";
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            var file = new FileContentResult(bytes, "application/csv_tickets")
            {
                FileDownloadName = "Tickets.csv"
            };

            return file;
        }
    }
}
