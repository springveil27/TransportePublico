namespace TransportePublicoRD.Application.Dto
{
    public class CreateScheduleDto
    {
 

        public TimeSpan StartTime { get; set; } // Format: "HH:mm"

        public TimeSpan EndTime { get; set; } // Format: "HH:mm"

        public int FrequencyMinutes { get; set; }
    }
}
