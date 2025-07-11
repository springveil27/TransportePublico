using TransportePublicoRD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TransportePublicoRD.Infrastructure.Interface;

namespace TransportePublicoRD.Infrastructure.Repositories
{
    //using generic repository to apply operation on schedule
    public class ScheduleRepository: GenericRepository<Schedule>, IScheduleRepository
    {
        //using generic repository to apply operation on schedule
    
        private readonly DbContextApp _context;

        
        public ScheduleRepository(DbContextApp context) : base(context)
        {
            _context = context;
        }

       
    }
}
