using Microsoft.AspNetCore.Mvc;
using TransportePublicoRD.Dto.RouteDto;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Repositories;
using TransportePublicoRD.Infrastructure.Interface;

namespace TransportePublicoRD.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        // create controller to manage public routes
        private readonly IUnitOfWork _unitOfWork;
        
        public RoutesController( IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }

        // function to get all routes
        [HttpGet]
       public async Task<IActionResult>  GetRoutes()
        {
            var routes = await _unitOfWork.RouteRepository.GetAllAsync();
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
            var route = await _unitOfWork.RouteRepository.GetByIdAsync(Id);

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
            await _unitOfWork.RouteRepository.AddAsync(route);
            await _unitOfWork.SaveAsync();
            return Ok(route);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(int id, [FromBody] UpdatePublicRouteDto request)
        {
            if (request == null || request.Id != id )
            {
                return BadRequest("Invalid route data.");
            }
            var existingRoutes = await _unitOfWork.RouteRepository.GetByIdAsync(request.Id);
            if (existingRoutes == null)
            {
                return NotFound($"Route with ID {existingRoutes.Id} not found.");
            }
            existingRoutes.Name = request.Name;
            existingRoutes.Code = request.Code;
            existingRoutes.Cost = request.Cost; 
           existingRoutes.UpdatedDate = DateTime.Now;
            existingRoutes.Active = request.Active;
          await _unitOfWork.RouteRepository.UpdateAsync(existingRoutes);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteRoute(int Id)
        {
            var routes = _unitOfWork.RouteRepository.GetByIdAsync(Id);
            if (routes == null)
            {
                return NotFound($"Route with ID {Id} not found.");
            }
           await _unitOfWork.RouteRepository.DeleteAsync(Id);
            return NoContent();
        }

        
    }
}
