
using Microsoft.EntityFrameworkCore;
using TransportePublicoRD.Domain.Entities;

namespace TransportePublicoRD.Infrastructure.Data.Repositories
{
   public class StopRepository : GenericRepository<Stop>
    {
     private readonly DbContextApp _context;
        public StopRepository(DbContextApp context) : base(context)
        {
            _context = context;
        }
       
    }
}
