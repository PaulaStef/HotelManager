using BusinessLayer.Contracts.Interfaces;
using BusinessLayer.Contracts.Models;
using LayersOnWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LayersOnWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [HttpPost("Make a reservation")]
        [Authorize(Roles = "Receptionist")]
        public string Post(Reservation reservation)
        {
            return reservationService.AddReservation(new ReservationModel {ClientName =  reservation.ClientName,
                                                                           ClientEmail = reservation.ClientEmail,
                                                                           CheckIn = reservation.CheckIn,
                                                                           CheckOut = reservation.CheckOut,
                                                                           Rooms = reservation.Rooms,
                                                                           Reduction = 0});
        }

        [HttpDelete("Cancel a reservation")]
        [Authorize(Roles = "Receptionist")]
        public string Delete(Guid id)
        {
            return reservationService.DeleteReservation(id);
        }

        [HttpPut("Update reservation")]
        [Authorize(Roles = "Receptionist")]
        public string UpdateReservation(Guid id, Reservation reservation)
        {
            return reservationService.UpdateReservation(id, new ReservationModel
            {
                ClientName = reservation.ClientName,
                ClientEmail = reservation.ClientEmail,
                CheckIn = reservation.CheckIn,
                CheckOut = reservation.CheckOut,
                Rooms = reservation.Rooms,
                Reduction = 0
            });
        }

        [HttpGet("Get all reservations")]
        [Authorize(Roles = "Receptionist")]
        public List<ReservationModel> getAll()
        {
            return reservationService.GetAll();
        }

        [HttpGet("Get reservation")]
        [Authorize(Roles = "Receptionist")]
        public ReservationModel GetReservation(Guid id)
        {
            return reservationService.GetById(id);
        }

        [HttpGet("Get price")]
        [Authorize(Roles = "Receptionist")]
        public string CalculatePrice([FromQuery]Guid[] rooms, string name, string email, int days)
        {
            return reservationService.CalculatePrice(rooms, name, email, days);
        }

        [HttpGet("Reservations from date")]
        [Authorize(Roles = "Administrator")]
        public IActionResult getAllFromDate(DateTime from, DateTime until, string type)
        {
            return reservationService.GetReservationsDate(from, until, type);
        }

        [HttpGet("Loyal Clients")]
        [Authorize(Roles = "Administrator")]
        public IActionResult getClients(string type)
        {
            return reservationService.GetLoyalClients(type);
        }
    }
}
