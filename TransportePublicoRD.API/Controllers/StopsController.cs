using Microsoft.AspNetCore.Mvc;
using TransportePublicoRD.Dto.StopsDto;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Repositories;

namespace TransportePublicoRD.Controllers
{
    [ApiController]
    [Route("api/routes/{routeId}/[controller]")]
    public class StopsController : ControllerBase
    {
   
        private readonly UnitOfWork _unitOfWork;

        public StopsController(StopRepository stopRepository, RouteRepository routeRepository, UnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetRouteStops(int routeId)
        {
            var route = await _unitOfWork.RouteRepository.GetByIdAsync(routeId);
            if (route == null)
                return NotFound($"Route with ID {routeId} not found.");

            var stops = await _unitOfWork.StopRepository.GetByIdAsync(routeId);
            return Ok(stops);
        }

        [HttpGet("{stopId}")]
        public async Task<IActionResult> GetStop( int stopId)
        {
            var stop = await _unitOfWork.StopRepository.GetByIdAsync(stopId);
            if (stop == null)
                return NotFound($"Stop with ID {stopId} not found.");

            return Ok(stop);
        }

        [HttpPost]
        public async Task<IActionResult> AddStopToRoute(int routeId, [FromBody] CreateStopDto request)
        {
            if (request == null)
                return BadRequest("Invalid stop data.");

            var route = await _unitOfWork.RouteRepository.GetByIdAsync(routeId);
            if (route == null)
                return NotFound($"Route with ID {routeId} not found.");

            var stop = new Stop
            {
                Name = request.Name,
                Address = request.Address,
                Order = request.Order,
                PublicRouteId = routeId
            };

            await _unitOfWork.StopRepository.AddAsync(stop);
            await _unitOfWork.SaveAsync();
            return Ok(stop);
        }

        [HttpPut("{stopId}")]
        public async Task<IActionResult> UpdateStop(int routeId, int stopId, [FromBody] UpdateStopDto request)
        {
            if (request == null)
                return BadRequest("Invalid stop data.");

            var stop = await _unitOfWork.StopRepository.GetByIdAsync(stopId);
            if (stop == null)
                return NotFound($"Stop with ID {stopId} not found in route {routeId}.");

            stop.Name = request.Name;
            stop.Address = request.Address;
            stop.Order = request.Order;

            await _unitOfWork.StopRepository.UpdateAsync(stop);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{stopId}")]
        public async Task<IActionResult> DeleteStop( int stopId)
        {
              await _unitOfWork.StopRepository.DeleteAsync(stopId);
            if (stopId == null )
                return NotFound($"Stop with ID {stopId} not found.");
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
