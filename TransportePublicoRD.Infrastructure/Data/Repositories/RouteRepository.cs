using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransportePublicoRD.Domain.Entities;

namespace TransportePublicoRD.Infrastructure.Data.Repositories
{
    public class RouteRepository : GenericRepository<PublicRoutes>
    {
        private readonly DbContextApp _context;
        public RouteRepository(DbContextApp context) : base(context)
        {
            _context = context;
        }
        
        
        

    }
}
