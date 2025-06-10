public class CreateScheduleDto
{    
    public DayOfWeek DayOfWeek { get; set; }

    public string StartTime { get; set; } // Format: "HH:mm"

    public string EndTime { get; set; } // Format: "HH:mm"

    public int FrequencyMinutes { get; set; }
}
