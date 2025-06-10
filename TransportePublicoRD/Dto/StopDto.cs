namespace TransportePublicoRD.Dto
{
    public class StopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Order { get; set; }
    }
}
