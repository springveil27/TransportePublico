using Microsoft.AspNetCore.Mvc;
using TransportePublicoRD.Entities;

namespace TransportePublicoRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private List<Line> _lines;
        public RoutesController()
        {
            // Initialize with some dummy data
            _lines = new List<Line>
            {

                new Line { Id = 1, Name = "Route A", Code = "A1", Cost = 10.0m, Description = "Route A Description" },
                new Line { Id = 2, Name = "Route B", Code = "B1", Cost = 15.0m, Description = "Route B Description" },
                new Line { Id = 3, Name = "Route C", Code = "C1", Cost = 20.0m, Description = "Route C Description" }
            };
        }

        [HttpGet]
        public IActionResult GetRoutes()
        {

            return Ok(_lines);
        }
        [HttpGet("{Id}")]
        public IActionResult GetRoute(int Id)
        {

            var line = new Line();

            line = _lines.FirstOrDefault(l => l.Id == Id);
            if (line == null)
            {
                return NotFound($"Route with ID {Id} not found.");
            }
            return Ok(line);
        }
        [HttpPost]
        public IActionResult CreateRoute([FromBody] Line line)
        {
            if (line == null)
            {
                return BadRequest("Invalid route data.");
            }
            line.Id = _lines.Count + 1;
            _lines.Add(line);
            return Ok(line);
        }

        [HttpPut]
        public IActionResult UpdateRoute([FromBody] Line Line)
        {
            if (Line == null)
            {
                return BadRequest("Invalid route data.");
            }
            var line = _lines.FirstOrDefault(l => l.Id == Line.Id);
            if (line == null)
            {
                return NotFound($"Route with ID {Line.Id} not found.");
            }
            line.Name = Line.Name;
            line.Code = Line.Code;
            line.Cost = Line.Cost;
            line.Description = Line.Description;
            line.UpdatedDate = DateTime.Now;
            return Ok(_lines);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteRoute(int Id)
        {
            var line = _lines.FirstOrDefault(l => l.Id == Id);
            if (line == null)
            {
                return NotFound($"Route with ID {Id} not found.");
            }
            _lines.Remove(line);
            return Ok(_lines);
        }

    }
}
