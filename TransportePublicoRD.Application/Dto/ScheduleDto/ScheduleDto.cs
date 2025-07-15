using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportePublicoRD.Application.Dto
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int FrequencyMinutes { get; set; }
        public int PublicRouteId { get; set; } // Assuming a foreign key to the route
        public string RouteName { get; set; } // Optional, for convenience
    }
}
