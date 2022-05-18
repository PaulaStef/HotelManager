using System;

namespace LayersOnWeb.Models
{
    public class Invoice
    {
        public Guid ReservationId { get; set; }
        public string Message { get; set; }
    }
}
