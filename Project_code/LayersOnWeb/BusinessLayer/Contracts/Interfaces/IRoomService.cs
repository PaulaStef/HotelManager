using BusinessLayer.Contracts.Models;
using DataAccess.Contracts;
using DataAccess.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IRoomService
    {
        string AddRoom(SingleRoomModel room);
        List<SingleRoomModel> GetAll();
        SingleRoomModel GetById(Guid id);
        SingleRoomEntity Model2Entity(SingleRoomModel room);
        List<ReservationModel> GetReservation(Guid id);
        void UpdateApartment(Guid id, int Price);
    }
}
