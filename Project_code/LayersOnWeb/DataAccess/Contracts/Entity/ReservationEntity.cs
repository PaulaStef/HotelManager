using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts.Entity
{
    public class ReservationEntity
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public int Reduction { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public virtual ICollection<RoomEntity> Rooms { get; set; }
    }
}
