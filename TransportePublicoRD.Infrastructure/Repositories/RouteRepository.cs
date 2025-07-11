using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Interface;

namespace TransportePublicoRD.Infrastructure.Repositories
{
    public class RouteRepository : GenericRepository<PublicRoutes>, IRouteRepository
    {
        //using generic repository to apply operation on publicRoute
        private readonly DbContextApp _context;
        public RouteRepository(DbContextApp context) : base(context)
        {
            _context = context;
        }
        
        
        

    }
}
