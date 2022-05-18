using DataAccess.Contracts;
using DataAccess.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private WeatherDbContext _dbContext;
        public IRepository<SingleRoomEntity> _singleRoomRepository { get; }
        public IRepository<ApartmentEntity> _apartmentRepository { get; }
        public IRepository<ReservationEntity> _reservationRepository { get; }
        public IRepository<InvoiceEntity> _invoiceRepository { get; }
        public UnitOfWork(WeatherDbContext dbContext, IRepository<SingleRoomEntity> roomRepository, IRepository<ApartmentEntity> apartmentRepository, IRepository<ReservationEntity> reservationRepository, IRepository<InvoiceEntity> _invoiceRepository)
        {
            this._dbContext = dbContext;
            this._singleRoomRepository = roomRepository;
            this._apartmentRepository = apartmentRepository;
            this._reservationRepository = reservationRepository;
            this._invoiceRepository = _invoiceRepository;
        }
        private bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                _dbContext.Dispose();
                this.disposed = true;
            }
            GC.SuppressFinalize(this);

        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
