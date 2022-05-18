using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts.Entity
{
    public class RoomEntity
    {
        public Guid ID { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public int PersonsNo { get; set; }
        public int Price { get; set; }
        public virtual ICollection<ReservationEntity> Reservations { get; set; }
    }
}
