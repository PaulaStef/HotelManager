using BusinessLayer.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessLayer.Contracts.Factory
{
    public static class XMLSerializeExtension
    {
        public static string ToXML<T>(this T o)
            where T : new()
        {
            string retVal;
            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(typeof(T));
                xs.Serialize(ms, o);
                ms.Flush();
                ms.Position = 0;
                var sr = new StreamReader(ms);
                retVal = sr.ReadToEnd();
            }
            return retVal;
        }
    }
    public class XmlDocument : IDocument 
    {
        public IActionResult Export<T>(List<T> entities) where T : class
        {
            string xml = entities.ToXML();

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(xml);
            var file = new FileContentResult(bytes, "application/csv_tickets")
            {
                FileDownloadName = "Reservations.xml"
            };

            return file;
        }

        public IActionResult Export(List<string> clients)
        {
            string xml = clients.ToXML();

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(xml);
            var file = new FileContentResult(bytes, "application/csv_tickets")
            {
                FileDownloadName = "Reservations.xml"
            };

            return file;
        }
    }
}
