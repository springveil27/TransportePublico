namespace TransportePublicoRD.Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public int PublicRouteId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int FrequencyMinutes { get; set; }

        public virtual PublicRoutes PublicRoute { get; set; }
    }
}
