using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Models
{
    public class ReservationModel
    {
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public int Reduction { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public virtual List<Guid> Rooms { get; set; }

        override
        public string ToString()
        {
            return ClientName + "," +
                   ClientEmail + "," +
                   Reduction + "," +
                   CheckIn + "," +
                   CheckOut + "," +
                   Rooms.Count;
        }
    }
}
