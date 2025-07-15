

namespace TransportePublicoRD.Application.Dto
{
    public class UpdateScheduleDto
    {

        public string StartTime { get; set; } // Format: "HH:mm"
        public string EndTime { get; set; } // Format: "HH:mm"
        public int FrequencyMinutes { get; set; }
    }
}
