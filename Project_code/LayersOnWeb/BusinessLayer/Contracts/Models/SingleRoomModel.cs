using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Models
{
    public class SingleRoomModel : RoomModel
    {
        public bool IsFromApartment { get; set; }
        public Guid ApartmentId { get; set; }
        public SingleRoomModel()
            : base()
        {
        }
        public override void Add(RoomModel c)
        {
            throw new NotImplementedException();
        }

        public override void Remove(RoomModel c)
        {
            throw new NotImplementedException();
        }
    }
}
