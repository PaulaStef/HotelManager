using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Models
{
    public class InvoiceModel
    {
        public Guid ReservationId { get; set; }
        public double TotalPrice { get; set; }
        public string Message { get; set; }
    }
}
