using Microsoft.AspNetCore.Mvc;
using TransportePublicoRD.Application.Dto;
using TransportePublicoRD.Application.Interface;
using TransportePublicoRD.Application.Services;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Repositories;

namespace TransportePublicoRD.Controllers
{
    [ApiController]
    [Route("api/routes/{routeId}/[controller]")]
    public class StopsController : ControllerBase
    {

        private readonly IStopsService _stopService;

        public StopsController(IStopsService stopsService)
        {

            _stopService = stopsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRouteStops(int routeId)
        {
            
            return Ok(await _stopService.GetRouteStops(routeId));
        }

        [HttpGet("/api/Stops/{stopId}")]
        public async Task<IActionResult> GetStop( int stopId)
        {

            return Ok(await _stopService.GetStop(stopId));
        }

        [HttpPost]
        public async Task<IActionResult> AddStopToRoute(int routeId, [FromBody] CreateStopDto request)
        {
           
            return Ok(await _stopService.AddStopToRoute(routeId,request));
        }

        [HttpPut("{stopId}")]
        public async Task<IActionResult> UpdateStop(int routeId, int stopId, [FromBody] UpdateStopDto request)
        {
            await _stopService.UpdateStop(routeId, stopId, request);
            return NoContent();
        }

        [HttpDelete("{stopId}")]
        public async Task<IActionResult> DeleteStop( int stopId)
        {
           await _stopService.DeleteStop(stopId);
            return NoContent();
        }
    }
}
