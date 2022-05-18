using DataAccess.Contracts.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class WeatherDbContext : IdentityDbContext<ApplicationUser>
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
       : base(options)
        {
        }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<SingleRoomEntity> SingleRooms { get; set; }
        public DbSet<ApartmentEntity> Apartments { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<InvoiceEntity> Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=.;Database=dbhotel;Trusted_Connection=True;");
        }
    }
}
