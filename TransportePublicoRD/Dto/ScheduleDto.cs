namespace TransportePublicoRD.Dto
{
    public class ScheduleDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int FrequencyMinutes { get; set; }
    }
}
