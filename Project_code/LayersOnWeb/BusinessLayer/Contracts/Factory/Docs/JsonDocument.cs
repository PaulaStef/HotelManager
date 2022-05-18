using BusinessLayer.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Factory.Docs
{
    public class JsonDocument : IDocument
    {
        public IActionResult Export<T>(List<T> entities) where T : class
        {
            var data = JsonSerializer.Serialize(entities);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);

            var file = new FileContentResult(bytes, "application/json_appointments")
            {
                FileDownloadName = "Appointments.json"
            };

            return file;
        }

        public IActionResult Export(List<string> clients)
        {
            var data = JsonSerializer.Serialize(clients);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);

            var file = new FileContentResult(bytes, "application/json_appointments")
            {
                FileDownloadName = "Appointments.json"
            };

            return file;
        }
    }
}
