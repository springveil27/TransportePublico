using System.ComponentModel.DataAnnotations.Schema;

namespace TransportePublicoRD.Domain.Entities
{
    public class PublicRoutes
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Stop> Stops { get; set; } = new List<Stop>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }

}
