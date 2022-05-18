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
    public class ApartmentService : IApartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string AddRoom(ApartmentModel room)
        {
            if (_unitOfWork._apartmentRepository.GetAll().Find(x => x.Number == room.Number && x.Floor == room.Floor) != null)
            {
                return "Room already exists.";
            }
            else 
            {
                if (_unitOfWork._singleRoomRepository.GetAll().Find(x => x.Number == room.Number && x.Floor == room.Floor) != null)
                {
                    return "Room already exists.";
                }
                else
                {
                    _unitOfWork._apartmentRepository.Add(Model2Entity(room));
                    _unitOfWork.SaveChanges();
                    return "Succes!";
                }
            }
        }
        public static ApartmentEntity Model2Entity(ApartmentModel room)
        {
            if(room == null)
            {
                return null;
            }
            return new ApartmentEntity
            {
                Number = room.Number,
                Floor = room.Floor,
                Price = room.Price,
                PersonsNo = room.PersonsNo
            };
        }
        public void UpdateApartment(int number,int Price, int PersonsNo, int NoOfRooms)
        {
            var existing = _unitOfWork._apartmentRepository.GetAll().Find(x => x.Number == number);
            if (existing != null)
            {
                existing.Price = Price;
                existing.PersonsNo = PersonsNo;
                existing.NoOfRooms = NoOfRooms;
                _unitOfWork._apartmentRepository.Update(existing);
                _unitOfWork.SaveChanges();
            }
        }

        public void UpdateApartment(Guid id, int Price)
        {
            var existing = _unitOfWork._apartmentRepository.GetAll().Find(x => x.ID == id);
            if (existing != null)
            {
                existing.Price = Price;
                _unitOfWork._apartmentRepository.Update(existing);
                _unitOfWork.SaveChanges();
            }
        }


        public  ApartmentModel Entity2Model(ApartmentEntity apartment)
        {
            var ap =  new ApartmentModel
            {
                Number = apartment.Number,
                Floor = apartment.Floor,
                Price = apartment.Price,
                NoOfRooms= apartment.NoOfRooms,
                PersonsNo= apartment.PersonsNo
            };
            var rooms = _unitOfWork._singleRoomRepository.GetAll().ToList();
            foreach (var room in rooms) {
                if (room.IsFromApartment)
                {
                    var x = _unitOfWork._apartmentRepository.GetAll().Select(x => x.ID == room.Apartment.ID).FirstOrDefault();
                    if (room.Apartment.ID == apartment.ID)
                    {
                        var newRoom = new SingleRoomModel
                        {
                            Floor = room.Floor,
                            IsFromApartment = room.IsFromApartment,
                            PersonsNo = room.PersonsNo,
                            Price = room.Price,
                            Number = room.Number,
                            ApartmentId = room.Apartment.ID
                    };
                        ap.Add(newRoom);
                    }
                }
            }
            return ap;
        }

        public List<ApartmentModel> GetAll()
        {
            List<ApartmentModel> labs = new List<ApartmentModel>();
            foreach (var lab in _unitOfWork._apartmentRepository.GetAll())
            {
                if (lab != null)
                {
                    labs.Add(Entity2Model(lab));
                }
            }
            return labs;
        }

        public ApartmentModel GetById(Guid id)
        {
            var existing = _unitOfWork._apartmentRepository.GetById(id);
            if (existing != null)
            {
                return Entity2Model(existing);
            }
            else
            {
                return null;
            }
        }

        public static bool isAvailable(ApartmentEntity room, DateTime from, DateTime to)
        {
            if (room.Reservations.Any(x => (from <= x.CheckIn) && (to >= x.CheckIn) && !x.Id.Equals(room.ID)))
            {
                System.Diagnostics.Debug.WriteLine("First Case");
                return false;
            }
            else
            {
                if (room.Reservations.Any(x => (from >= x.CheckIn) && (from <= x.CheckOut) && x.Id != room.ID))
                {
                    System.Diagnostics.Debug.WriteLine("Second Case" );
                    return false;
                }
                else
                {
                    if(room.Reservations.Any(x => (from <= x.CheckIn) && (to >= x.CheckOut) && x.Id != room.ID))
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
            var room = _unitOfWork._apartmentRepository.GetAllQueryable().Where(x => x.ID == id).Include(x => x.Reservations).FirstOrDefault();
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
