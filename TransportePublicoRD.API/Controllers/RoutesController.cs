using Microsoft.AspNetCore.Mvc;
using TransportePublicoRD.Dto.RouteDto;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Data.Repositories;

namespace TransportePublicoRD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
      private readonly RouteRepository _routeRepository;
        public RoutesController(RouteRepository routeRepository)
        {
            _routeRepository = routeRepository;

        }

        [HttpGet]
       public async Task<IActionResult>  GetRoutes()
        {
            var routes = await _routeRepository.GetAllAsync();
            if (routes == null || !routes.Any())
            {
                return NotFound("No routes found.");
            }
            var routesWithDetails = routes.Select
                (r => new
                {
                    r.Id,
                    r.Name,
                    r.Code,
                    r.Cost,
                    r.Active,
                   
                    Stops = r.Stops.OrderBy(s => s.Order).Select(s => new
                    {
                        s.Id,
                        s.Name,
                        s.Order
                    }),
                    Schedules = r.Schedules.Select(s => new
                    {
                        s.Id,
                        s.StartTime,
                        s.EndTime
                    })
                }).ToList();
            return Ok(routesWithDetails);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetRouteById(int Id)
        {
            var route = await _routeRepository.GetByIdAsync(Id);

            if (route == null)
            {
                return NotFound($"Route with ID {Id} not found.");
            }

            return Ok(route);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody]  CreatePublicRouteDto request)
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
            await _routeRepository.AddAsync(route);
            return Ok(route);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(int id, [FromBody] UpdatePublicRouteDto request)
        {
            if (request == null || request.Id != id )
            {
                return BadRequest("Invalid route data.");
            }
            var existingRoutes = await _routeRepository.GetByIdAsync(request.Id);
            if (existingRoutes == null)
            {
                return NotFound($"Route with ID {existingRoutes.Id} not found.");
            }
            existingRoutes.Name = request.Name;
            existingRoutes.Code = request.Code;
            existingRoutes.Cost = request.Cost; 
           existingRoutes.UpdatedDate = DateTime.Now;
            existingRoutes.Active = request.Active;
          await _routeRepository.UpdateAsync(existingRoutes);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteRoute(int Id)
        {
            var routes = _routeRepository.GetByIdAsync(Id);
            if (routes == null)
            {
                return NotFound($"Route with ID {Id} not found.");
            }
           await _routeRepository.DeleteAsync(Id);
            return NoContent();
        }

        
    }
}
