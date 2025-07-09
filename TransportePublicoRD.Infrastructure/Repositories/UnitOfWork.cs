using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportePublicoRD.Infrastructure.Interface;

namespace TransportePublicoRD.Infrastructure.Repositories
{
    // unit of work pattern to manage repositories and transactions
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContextApp _context;
        private IRouteRepository _RouteRepository;
        private IScheduleRepository _ScheduleRepository;
        private IStopRepository _stopRepository;
        public UnitOfWork(DbContextApp context, IRouteRepository routeRepository, IScheduleRepository scheduleRepository, IStopRepository stopRepository)
        {
            _context = context;
            _RouteRepository = routeRepository;
            _ScheduleRepository = scheduleRepository;
            _stopRepository = stopRepository;
        }
        public IRouteRepository RouteRepository => _RouteRepository ?? throw new ArgumentException();
        public IScheduleRepository ScheduleRepository => _ScheduleRepository ?? throw new ArgumentException();
        public IStopRepository StopRepository => _stopRepository ?? throw new ArgumentException();

        public async Task BeginTransactionAsync()
        {
           await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
