
namespace TransportePublicoRD.Application.Dto
{
    public class RouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Cost { get; set; }
        public bool Active { get; set; }
        public List<StopsDto> Stops { get; set; } = new List<StopsDto>();
        public List<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();

    }
}
