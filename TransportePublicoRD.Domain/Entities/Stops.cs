namespace TransportePublicoRD.Domain.Entities
{
    public class Stop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Order { get; set; } 

        public int PublicRouteId { get; set; }
        public virtual PublicRoutes PublicRoute { get; set; }
    }
}
