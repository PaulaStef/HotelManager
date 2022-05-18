using BusinessLayer.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IApartmentService
    {
        string AddRoom(ApartmentModel room);
        List<ApartmentModel> GetAll();
        ApartmentModel GetById(Guid id);
        void UpdateApartment(int number, int Price, int PersonsNo, int NoOfRooms);
        void UpdateApartment(Guid id, int Price);
        List<ReservationModel> GetReservation(Guid id);
    }


}
