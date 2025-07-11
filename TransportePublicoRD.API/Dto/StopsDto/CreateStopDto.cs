using System.ComponentModel.DataAnnotations;

namespace TransportePublicoRD.Dto.StopsDto
{
    public class CreateStopDto
    {

        public string Name { get; set; }


        public string Address { get; set; }

        public int Order { get; set; }
}   }
