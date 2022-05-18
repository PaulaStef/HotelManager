using DataAccess.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        IRepository<SingleRoomEntity> _singleRoomRepository { get; }
        IRepository<ApartmentEntity> _apartmentRepository { get; }
        IRepository<ReservationEntity> _reservationRepository { get; }
        IRepository<InvoiceEntity> _invoiceRepository { get; }
        void SaveChanges();
        void Dispose();
    }
}
