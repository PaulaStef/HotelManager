using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts.Entity
{
    public class InvoiceEntity
    {
        public Guid Id { get; set; }
        public virtual ReservationEntity Reservation { get; set; }
        public double TotalPrice { get; set; }
        public string Message { get; set; }
    }
}
