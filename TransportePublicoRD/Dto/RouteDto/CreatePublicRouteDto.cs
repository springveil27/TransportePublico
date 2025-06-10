using System.ComponentModel.DataAnnotations;

namespace TransportePublicoRD.Dto.RouteDto
{
    public class CreatePublicRouteDto
    {
    
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Cost { get; set; }
        public bool Active { get; set; } = true;
    }
}
