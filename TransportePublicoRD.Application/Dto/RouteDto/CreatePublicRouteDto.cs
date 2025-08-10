
namespace TransportePublicoRD.Application.Dto
{
    public class CreatePublicRouteDto
    {
    
        public string Name { get; set; }
        public string Code { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Cost { get; set; }
        public bool Active { get; set; } = true;
    }
}
