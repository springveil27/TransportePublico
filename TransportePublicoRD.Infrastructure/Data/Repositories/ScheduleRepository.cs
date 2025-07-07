using TransportePublicoRD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TransportePublicoRD.Infrastructure.Data.Repositories
{
    public class ScheduleRepository: GenericRepository<Schedule>
    {
        private readonly DbContextApp _context;

        public ScheduleRepository(DbContextApp context) : base(context)
        {
            _context = context;
        }

       
    }
}
