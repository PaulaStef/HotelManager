using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts.Entity
{
    public class SingleRoomEntity : RoomEntity
    {
        public bool IsFromApartment { get; set; }
        public virtual ApartmentEntity Apartment { get; set; }
    }
}
