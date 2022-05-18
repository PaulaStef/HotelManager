using BusinessLayer.Contracts.Interfaces;
using BusinessLayer.Contracts.Models;
using DataAccess.Contracts;
using DataAccess.Contracts.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApartmentService _apartmentService;
        public RoomService(IUnitOfWork unitOfWork, IApartmentService apartmentService)
        {
            this._unitOfWork = unitOfWork;
            this._apartmentService = apartmentService;
        }
        public string AddRoom(SingleRoomModel room)
        {
            if(_unitOfWork._singleRoomRepository.GetAll().Find(x => x.Number == room.Number && x.Floor == room.Floor) != null && 
                !room.IsFromApartment)
            {
                return "Room already exists.";
            }
            else
            {
                if (room.IsFromApartment && _apartmentService.GetById(room.ApartmentId) == null)
                {
                    return "Apartment not found.";
                }
                else
                {
                    if (_unitOfWork._apartmentRepository.GetAll().Find(x => x.Number == room.Number && x.Floor == room.Floor) != null)
                    {
                        return "Room already exists.";
                    }
                    else
                    {
                        _unitOfWork._singleRoomRepository.Add(Model2Entity(room));
                        _unitOfWork.SaveChanges();
                    }
                    if (room.IsFromApartment)
                    {
                        var apartment = _apartmentService.GetById(room.ApartmentId);
                        if (apartment != null)
                        {
                            apartment.PersonsNo += room.PersonsNo;
                            apartment.NoOfRooms++;
                            _apartmentService.UpdateApartment(apartment.Number, apartment.CalculatePrice(), apartment.PersonsNo, apartment.NoOfRooms);
                        }
                    }
                    return "Succes!";
                }
            }
        }
        public void UpdateApartment(Guid id, int Price)
        {
            var existing = _unitOfWork._singleRoomRepository.GetAll().Find(x => x.ID == id);
            if (existing != null)
            {
                existing.Price = Price;
                _unitOfWork._singleRoomRepository.Update(existing);
                _unitOfWork.SaveChanges();
            }
        }
        public SingleRoomModel GetById(Guid id)
        {
            var existing = _unitOfWork._singleRoomRepository.GetById(id);
            if (existing != null)
            {
                return Entity2Model(existing);
            }
            else
            {
                return null;
            }
        }
        public  SingleRoomEntity Model2Entity(SingleRoomModel room)
        {
            ApartmentEntity apartment;
            if (room.IsFromApartment)
            {
                apartment = _unitOfWork._apartmentRepository.GetById(room.ApartmentId);
            }
            else
            {
                apartment = null;
            }
            var newRoom = new SingleRoomEntity
            {
                Number = room.Number,
                Floor = room.Floor,
                Price = room.Price,
                PersonsNo = room.PersonsNo,
                IsFromApartment = room.IsFromApartment,
                Apartment = apartment
            };

            if (room.IsFromApartment)
            {
                newRoom.Number = apartment.Number;
                newRoom.Floor = apartment.Floor;
            }
            return newRoom;
        }

        public SingleRoomModel Entity2Model(SingleRoomEntity room)
        {
            var newRoom = new SingleRoomModel
            {
                Floor = room.Floor,
                IsFromApartment = room.IsFromApartment,
                PersonsNo = room.PersonsNo,
                Price = room.Price,
                Number = room.Number,
            };
            if (room.IsFromApartment)
            {   var apartment = _unitOfWork._apartmentRepository.GetAll().Select(x => x.ID == room.Apartment.ID).FirstOrDefault();
                newRoom.ApartmentId =  room.Apartment.ID;
            }
            return newRoom;
        }

        public List<SingleRoomModel> GetAll()
        {
            List<SingleRoomModel> rooms = new List<SingleRoomModel>();
            foreach (var room in _unitOfWork._singleRoomRepository.GetAll())
            {
                if (room != null)
                {
                    rooms.Add(Entity2Model(room));
                }
            }
            return rooms;
        }

        public static bool isAvailable(SingleRoomEntity room, DateTime from, DateTime to)
        {
            if (room.Reservations.Any(x => (from <= x.CheckIn) && (to >= x.CheckIn) && x.Id != room.ID))
            {
                System.Diagnostics.Debug.WriteLine("First Case");
                return false;
            }
            else
            {
                if (room.Reservations.Any(x => (from >= x.CheckIn) && (from <= x.CheckOut) && x.Id != room.ID))
                {
                    System.Diagnostics.Debug.WriteLine("Second Case");
                    return false;
                }
                else
                {
                    if (room.Reservations.Any(x => (from <= x.CheckIn) && (to >= x.CheckOut) && x.Id != room.ID))
                    {
                        System.Diagnostics.Debug.WriteLine("Third Case");
                        return false;
                    }
                }
            }

            return true;
        }

        public List<ReservationModel> GetReservation(Guid id)
        {
            List<ReservationModel> reservations = new List<ReservationModel>();
            var room = _unitOfWork._singleRoomRepository.GetAllQueryable().Where(x => x.ID == id).Include(x => x.Reservations).FirstOrDefault();
            if (room != null)
            {
                foreach (var reservation in room.Reservations)
                {
                    reservations.Add(ReservationService.Entity2Model(reservation));
                }
            }

            return reservations;
        }
    }
}
