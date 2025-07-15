

namespace TransportePublicoRD.Application.Dto
{
    public class StopsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Order { get; set; } // Order of the stop in the route
        public int PublicRouteId { get; set; } // Foreign key to the route
        public string PublicRouteName { get; set; } // Optional, for convenience
    }
}
