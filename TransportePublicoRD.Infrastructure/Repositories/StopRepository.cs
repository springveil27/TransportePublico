using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Interface;


//using generic repository to apply operation on stop
namespace TransportePublicoRD.Infrastructure.Repositories
{
   public class StopRepository : GenericRepository<Stop>, IStopRepository
    {
     private readonly DbContextApp _context;
        public StopRepository(DbContextApp context) : base(context)
        {
            _context = context;
        }
       
    }
}
