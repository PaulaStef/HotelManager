using BusinessLayer.Contracts.Interfaces;
using BusinessLayer.Contracts.Models;
using BusinessLayer.Contracts.Services;
using LayersOnWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LayersOnWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SingleRoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public SingleRoomController(IRoomService _roomService)
        {
            this._roomService = _roomService;
        }

        [HttpPost("Add Room")]
        [Authorize(Roles = "Administrator")]
        public string Post( Room room)
        { 
            return _roomService.AddRoom(new SingleRoomModel { Floor = room.Floor,
                                                              Price = room.Price,
                                                              Number = room.Number,
                                                              PersonsNo = room.PersonsNo,
                                                              IsFromApartment = false});
        }

        [HttpPost("Add Room For Apartment")]
        [Authorize(Roles = "Administrator")]
        public string Post(RoomApartment room)
        {
            return _roomService.AddRoom(new SingleRoomModel
            {
                Floor = 0,
                Price = room.Price,
                Number = 0,
                PersonsNo = room.PersonsNo,
                IsFromApartment = true,
                ApartmentId = room.ApartmentId
            }) ;
        }

        [HttpPost("Update Price")]
        [Authorize(Roles = "Administrator")]
        public void UpdatePrice(Guid id, int price)
        {
            _roomService.UpdateApartment(id, price);
        }

        [HttpGet("Get rooms")]
        [Authorize(Roles = "Receptionist")]
        public List<SingleRoomModel> GetAll()
        {
            return _roomService.GetAll();
        }

        [HttpGet("Get room")]
        [Authorize(Roles = "Receptionist")]
        public SingleRoomModel GetById(Guid id)
        {
            return _roomService.GetById(id);
        }

        [HttpGet("Get reservations")]
        [Authorize(Roles = "Receptionist")]
        public List<ReservationModel> GetReservations(Guid id)
        {
            return _roomService.GetReservation(id);
        }
    }
}
