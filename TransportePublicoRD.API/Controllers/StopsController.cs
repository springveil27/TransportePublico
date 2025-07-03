using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportePublicoRD.Infrastructure.Data;
using TransportePublicoRD.Dto;
using TransportePublicoRD.Dto.StopsDto;
using TransportePublicoRD.Domain.Entities;

namespace TransportePublicoRD.Controllers
{
    [ApiController]
    [Route("api/routes/{routeId}/[controller]")]
    public class StopsController : ControllerBase
    {
        private readonly DbContextApp _context;

        public StopsController(DbContextApp context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetRouteStops(int routeId)
        {
            var route = _context.PublicRoutes.FirstOrDefault(r => r.Id == routeId);
            if (route == null)
            {
                return NotFound($"Route with ID {routeId} not found.");
            }

            var stops = _context.Stops
                .Where(s => s.PublicRouteId == routeId)
                .OrderBy(s => s.Order)
                .ToList();

            return Ok(stops);
        }

        [HttpGet("{stopId}")]
        public IActionResult GetStop(int routeId, int stopId)
        {
            var stop = _context.Stops
                .FirstOrDefault(s => s.Id == stopId && s.PublicRouteId == routeId);

            if (stop == null)
            {
                return NotFound($"Stop with ID {stopId} not found in route {routeId}.");
            }

            return Ok(stop);
        }

        [HttpPost]
        public IActionResult AddStopToRoute(int routeId, [FromBody] CreateStopDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid stop data.");
            }

            var route = _context.PublicRoutes.FirstOrDefault(r => r.Id == routeId);
            if (route == null)
            {
                return NotFound($"Route with ID {routeId} not found.");
            }

            var stop = new Stop
            {
                Name = request.Name,
                Address = request.Address,
                Order = request.Order,
                PublicRouteId = routeId
            };

            _context.Stops.Add(stop);
            _context.SaveChanges();

            return Ok(stop);
        }

        [HttpPut("{stopId}")]
        public IActionResult UpdateStop(int routeId, int stopId, [FromBody] UpdateStopDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid stop data.");
            }

            var stop = _context.Stops
                .FirstOrDefault(s => s.Id == stopId && s.PublicRouteId == routeId);

            if (stop == null)
            {
                return NotFound($"Stop with ID {stopId} not found in route {routeId}.");
            }

            stop.Name = request.Name;
            stop.Address = request.Address;
            stop.Order = request.Order;

            _context.Update(stop);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{stopId}")]
        public IActionResult DeleteStop(int routeId, int stopId)
        {
            var stop = _context.Stops
                .FirstOrDefault(s => s.Id == stopId && s.PublicRouteId == routeId);

            if (stop == null)
            {
                return NotFound($"Stop with ID {stopId} not found in route {routeId}.");
            }

            _context.Remove(stop);
            _context.SaveChanges();

            return NoContent();
        }
    }
}