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
    public class ApartmentController : ControllerBase
    {
        private readonly IApartmentService _apartmentservice;
        public ApartmentController(IApartmentService apartmentService)
        {
            _apartmentservice = apartmentService;
        }

        [HttpPost("Add apartments")]
        [Authorize(Roles = "Administrator")]
        public string Post(Apartment assignment)
        {
            return _apartmentservice.AddRoom(new ApartmentModel
            {
                Floor = assignment.Floor,
                Number = assignment.Number,
                PersonsNo = 0,
                Price = 0
            });
        }

        [HttpPut("Update Price")]
        [Authorize(Roles = "Administrator")]
        public void UpdatePrice(Guid id, int price)
        {
            _apartmentservice.UpdateApartment(id, price);
        }

        [HttpGet("Get all")]
        [Authorize(Roles = "Receptionist")]
        public List<ApartmentModel> GetAll()
        {
            return _apartmentservice.GetAll();
        }
        [HttpGet("Get apartment")]
        [Authorize(Roles = "Receptionist")]
        public ApartmentModel GetById(Guid id)
        {
            return _apartmentservice.GetById(id);
        }

        [HttpGet("Get reservations")]
        [Authorize(Roles = "Receptionist")]
        public List<ReservationModel> GetReservations(Guid id)
        {
            return _apartmentservice.GetReservation(id);
        }

    }
}
