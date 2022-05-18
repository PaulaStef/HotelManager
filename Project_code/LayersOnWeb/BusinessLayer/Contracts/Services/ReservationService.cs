using BusinessLayer.Contracts.Factory;
using BusinessLayer.Contracts.Interfaces;
using BusinessLayer.Contracts.Models;
using DataAccess.Contracts;
using DataAccess.Contracts.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDocumentFactory documentFactory;    
        public ReservationService(IUnitOfWork unitOfWork, IDocumentFactory documentFactory)
        {
            this.unitOfWork = unitOfWork;
            this.documentFactory = documentFactory;
        }
        public string AddReservation(ReservationModel reservation)
        {
            var res = Model2Entity(reservation);
            ICollection<RoomEntity> roomModels = new List<RoomEntity>();
            foreach (Guid id in reservation.Rooms)
            {
                var room = unitOfWork._apartmentRepository.GetAllQueryable().Where(x => x.ID == id).Include(x => x.Reservations).FirstOrDefault();
                if (room == null)
                {
                    var room2 = unitOfWork._singleRoomRepository.GetAllQueryable().Where(x => x.ID == id).Include(x => x.Reservations).FirstOrDefault();
                    if (room2 == null)
                    {
                        return "Room " + id + " not found";
                    }
                    else
                    {
                        if (room2.IsFromApartment)
                        {
                            return "Room is from apartment";
                        }
                        if (RoomService.isAvailable(room2,reservation.CheckIn, reservation.CheckOut)) {
                            roomModels.Add(room2);
                        }
                        else
                        {
                            return "Room not available";
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(room.Number);
                    if (ApartmentService.isAvailable(room, reservation.CheckIn, reservation.CheckOut))
                    {
                        roomModels.Add(room);
                    }
                    else
                    {
                        return "Apartment not available";
                    }
                }

            }
            res.Rooms = roomModels;
            unitOfWork._reservationRepository.Add(res);
            unitOfWork.SaveChanges();
            return "Succes";
        }

        public string CalculatePrice(Guid[] rooms, string name, string email, int days)
        {
            int reduction = CheckForReduction(name, email);
            double price = 0;
            foreach(var id in rooms)
            {
                var room = unitOfWork._apartmentRepository.GetById(id);
                if(room == null)
                {
                    var room2 = unitOfWork._singleRoomRepository.GetById(id);
                    if (room2 == null)
                    {
                        return "Room not found";
                    }
                    else
                    {
                        price += room2.Price;
                    }
                }
                else
                {
                    price += room.Price;
                }
            }
            double reduct = price * (reduction / 100.0);
            price -= reduct;
            price *= days;
            return price.ToString();
        }

        public int CheckForReduction(string name, string email)
        {
            var reservations = GetAll().Where(x => x.ClientName.Equals(name) && x.ClientEmail.Equals(email));
            if(reservations.Count() > 2)
            {
                return 30;
            }
            return 0;
        } 

        public string DeleteReservation(Guid id)
        {
            var existing = unitOfWork._reservationRepository.GetById(id);
            if(existing != null)
            {
                unitOfWork._reservationRepository.Delete(existing);
                unitOfWork.SaveChanges();
                return "Reservation Deleted";
            }
            else
            {
                return "Reservation not found";
            }
            
        }

        public string UpdateReservation(Guid Reservationid, ReservationModel reservation)
        {
            var existing = unitOfWork._reservationRepository.GetById(Reservationid);
            if(existing == null)
            {
                return "Reservation not found!";
            }
            else
            {
                existing.CheckOut = reservation.CheckOut;
                existing.CheckIn = reservation.CheckIn;
                ICollection<RoomEntity> roomModels = new List<RoomEntity>();
                foreach (Guid id in reservation.Rooms)
                {
                    var room = unitOfWork._apartmentRepository.GetAllQueryable().Where(x => x.ID == id).Include(x => x.Reservations).FirstOrDefault();
                    if (room == null)
                    {
                        var room2 = unitOfWork._singleRoomRepository.GetAllQueryable().Where(x => x.ID == id).Include(x => x.Reservations).FirstOrDefault();
                        if (room2 == null)
                        {
                            return "Room " + id + " not found";
                        }
                        else
                        {
                            if (room2.IsFromApartment)
                            {
                                return "Room is from apartment";
                            }
                            if (RoomService.isAvailable(room2, reservation.CheckIn, reservation.CheckOut))
                            {
                                roomModels.Add(room2);
                            }
                            else
                            {
                                return "Room is not available";
                            }
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(room.Number);
                        if (ApartmentService.isAvailable(room, reservation.CheckIn, reservation.CheckOut))
                        {
                            roomModels.Add(room);
                        }
                        else
                        {
                            return "Room is not available";
                        }
                    }

                }
                existing.Rooms = roomModels;
            }
            unitOfWork._reservationRepository.Update(existing);
            unitOfWork.SaveChanges();
            return "Succes";
            
        }

        public List<ReservationModel> GetAll()
        {
            List<ReservationModel> reservations = new List<ReservationModel>();
            foreach (var reservation in unitOfWork._reservationRepository.GetAllQueryable().Include(x => x.Rooms))
            {
                if (reservation != null)
                {
                    reservations.Add(Entity2Model(reservation));
                }
            }
            return reservations; 
        }

        public static ReservationModel Entity2Model(ReservationEntity reservation)
        {
            List<Guid> roomId = new List<Guid>();
            foreach(var room in reservation.Rooms)
            {
                roomId.Add(room.ID);
            }
            return new ReservationModel
            {
                ClientName = reservation.ClientName,
                ClientEmail = reservation.ClientEmail,
                CheckIn = reservation.CheckIn,
                CheckOut = reservation.CheckOut,
                Reduction = reservation.Reduction,
                Rooms = roomId
            };
        }

        public ReservationModel GetById(Guid id)
        {
            var existing = unitOfWork._reservationRepository.GetAllQueryable().Where(x => x.Id == id).Include(x => x.Rooms).FirstOrDefault();
            if (existing != null)
            {
                return Entity2Model(existing);
            }
            else
            {
                return null;
            }
        }
        public ReservationEntity Model2Entity(ReservationModel reservation)
        {
            return new ReservationEntity
            {
                ClientName = reservation.ClientName,
                ClientEmail = reservation.ClientEmail,
                CheckIn = reservation.CheckIn,
                CheckOut = reservation.CheckOut,
                Reduction = CheckForReduction(reservation.ClientName, reservation.ClientEmail)
            };
        }
        
        public IActionResult GetLoyalClients(string type)
        {
            var list = unitOfWork._reservationRepository.GetAll();
            List<string> loyalclients = new List<string>();
            var clients = list.GroupBy(i => i.ClientEmail).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key);
            int i=0;
            foreach (var client in clients)
            {   if (i < 10)
                {
                    i++;
                    loyalclients.Add(client);
                    System.Diagnostics.Debug.WriteLine(client);
                }
            }
            return documentFactory.CreateExport(type).Export(loyalclients);
        }
        public IActionResult GetReservationsDate(DateTime cheackIn, DateTime checkOut, string type)
        {
            List<ReservationModel> reservations = new List<ReservationModel>();
            foreach (var reservation in unitOfWork._reservationRepository.GetAllQueryable().Where(x => DateTime.Compare(x.CheckIn, cheackIn) >= 0 && DateTime.Compare(x.CheckOut, checkOut) <=0).Include(x => x.Rooms))
            {
                if (reservation != null)
                {
                    reservations.Add(Entity2Model(reservation));
                }
            }
            return documentFactory.CreateExport(type).Export<ReservationModel>(reservations);
        }
 
    }
}
