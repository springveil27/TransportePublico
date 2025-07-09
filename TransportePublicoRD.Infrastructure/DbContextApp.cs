using TransportePublicoRD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TransportePublicoRD.Infrastructure;

public class DbContextApp:DbContext
{

    public DbContextApp(DbContextOptions<DbContextApp> options) : base(options)
    { 
    }
    public DbSet<PublicRoutes> PublicRoutes { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Schedule> Schedules { get; set; }

}
