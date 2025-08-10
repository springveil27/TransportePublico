using Microsoft.AspNetCore.Mvc;
using TransportePublicoRD.Application.Dto;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Repositories;
using TransportePublicoRD.Infrastructure.Interface;
using TransportePublicoRD.Application.Interface;


namespace TransportePublicoRD.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        // create controller to manage public routes
        private readonly IRouteService _routeService;
        public RoutesController( IRouteService routeService)
        {
            _routeService = routeService;
        }

        // function to get all routes
        [HttpGet]
       public async Task<IActionResult>  GetRoutes()
        {
           
            return Ok(await _routeService.GetRoutes());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetRouteById(int Id)
        {
   
            return Ok(await _routeService.GetRouteById(Id));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRoutes(
            [FromQuery] string? name = null,
            [FromQuery] string? origin = null,
           [FromQuery] string? destination = null)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(origin) && string.IsNullOrEmpty(destination))
            {
                return BadRequest("At least one search parameter (name, origin, or destination) must be provided.");
            }

            var routes = await _routeService.SearchRoutes(name, origin, destination);
            return Ok(routes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody]  CreatePublicRouteDto request)
        {
         var routeId = await _routeService.CreateRoute(request);
            return Ok(routeId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute( int id, [FromBody] UpdatePublicRouteDto request)
        {
            if(id != request.Id)
            {
                return BadRequest("la ruta en la url no coincide con la del cuerpo.");
            }
            
            await _routeService.UpdateRoute( request);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteRoute(int Id)
        {
            
          await _routeService.DeleteRoute(Id);
            return NoContent();
        }

        
    }
}
