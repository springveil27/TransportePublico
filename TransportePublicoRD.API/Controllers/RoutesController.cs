using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TransportePublicoRD.Infrastructure.Data;
using TransportePublicoRD.Dto.RouteDto;
using TransportePublicoRD.Domain.Entities;

namespace TransportePublicoRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly DbContextApp _context;
        public RoutesController(DbContextApp context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult  GetRoutes()
        {
            var routes = _context.PublicRoutes
                .Include(r => r.Stops.OrderBy(s => s.Order))
                .Include(r => r.Schedules)
                .Where(r => r.Active)
                .ToList();

            return Ok(routes);
        }

        [HttpGet("{Id}")]
        public IActionResult GetRoute(int Id)
        {
            var routes = new PublicRoutes();

            routes = _context.PublicRoutes
               .Include(r => r.Stops.OrderBy(s => s.Order))
               .Include(r => r.Schedules)
               .FirstOrDefault(r => r.Id == Id);

            if (routes == null)
            {
                return NotFound($"Route with ID {Id} not found.");
            }
            return Ok(routes);

        }
        [HttpPost]
        public IActionResult CreateRoute([FromBody]  CreatePublicRouteDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid route data.");
            }
            var route = new PublicRoutes
            {
                Name = request.Name,
                Code = request.Code,
                Cost = request.Cost,
                Active = request.Active,
                CreatedDate = DateTime.Now
            };
            _context.PublicRoutes.Add(route);
            _context.SaveChanges();
            return Ok(route);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoute(int id, [FromBody] UpdatePublicRouteDto request)
        {
            if (request == null || request.Id != id )
            {
                return BadRequest("Invalid route data.");
            }
            var existingRoutes = _context.PublicRoutes.FirstOrDefault(l => l.Id == id);
            if (existingRoutes == null)
            {
                return NotFound($"Route with ID {existingRoutes.Id} not found.");
            }
            existingRoutes.Name = request.Name;
            existingRoutes.Code = request.Code;
            existingRoutes.Cost = request.Cost; 
           existingRoutes.UpdatedDate = DateTime.Now;
            existingRoutes.Active = request.Active;
            _context.Update(existingRoutes);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteRoute(int Id)
        {
            var routes = _context.PublicRoutes.FirstOrDefault(l => l.Id == Id);
            if (routes == null)
            {
                return NotFound($"Route with ID {Id} not found.");
            }
            _context.Remove(routes);
             _context.SaveChanges();
            return NoContent();
        }

        
    }
}
