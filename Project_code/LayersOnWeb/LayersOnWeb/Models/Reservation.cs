using System;
using System.Collections.Generic;

namespace LayersOnWeb.Models
{
    public class Reservation
    {
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public virtual List<Guid> Rooms { get; set; }
    }
}
