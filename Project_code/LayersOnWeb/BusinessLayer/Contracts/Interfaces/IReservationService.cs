using BusinessLayer.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IReservationService
    {
        string AddReservation(ReservationModel room);
        string DeleteReservation(Guid id);
        string UpdateReservation(Guid id, ReservationModel room);
        string CalculatePrice(Guid[] rooms, string name, string email, int days);
        int CheckForReduction(string name, string email); 
        List<ReservationModel> GetAll();
        IActionResult GetReservationsDate(DateTime cheackIn, DateTime checkOut, string type);
        ReservationModel GetById(Guid id);
        IActionResult GetLoyalClients(string type);
    }
}
