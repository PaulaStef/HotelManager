using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Models
{
    public class ApartmentModel : RoomModel
    {
        public List<RoomModel> _rooms = new List<RoomModel>();
        public int NoOfRooms { get; set; }
        public ApartmentModel()
            : base()
        {
        }
        public override void Add(RoomModel component)
        {
            _rooms.Add(component);
        }
        public override void Remove(RoomModel component)
        {
            _rooms.Remove(component);
        }

        public int CalculatePrice()
        {
            int sum = 0;
            foreach (var room in _rooms)
            {
                sum += room.Price;
            }
            return sum;
        }
    }
}
