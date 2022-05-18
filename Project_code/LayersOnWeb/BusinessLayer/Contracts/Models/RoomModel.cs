using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Models
{
    public abstract class RoomModel
    {
        public int Number { get; set; }
        public int Floor { get; set; }
        public int PersonsNo { get; set; }
        public int Price { get; set; }
        public abstract void Add(RoomModel c);
        public abstract void Remove(RoomModel c);
    }
}
