using Microsoft.AspNetCore.Mvc;
using TransportePublicoRD.Application.Dto;
using TransportePublicoRD.Application.Interface;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Repositories;

namespace TransportePublicoRD.Controllers
{
    [ApiController]
    [Route("api/routes/{routeId}/[controller]")]
    public class SchedulesController : ControllerBase
    {
      
       private readonly ISchedulesService _scheduleService;

        public SchedulesController(ISchedulesService schedulesService)
        {
        
            _scheduleService = schedulesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRouteSchedules(int routeId)
        {
           
            return Ok(await _scheduleService.GetRouteSchedules(routeId));
        }

        [HttpGet("/api/schedules/{scheduleId}")]
        public async Task<IActionResult> GetSchedule( int scheduleId)
        {

            return Ok(await _scheduleService.GetSchedule(scheduleId));
        }

        [HttpPost]
        public async Task<IActionResult> AddScheduleToRoute(int routeId, [FromBody] CreateScheduleDto request)
        {
            var scheduleId = await _scheduleService.AddScheduleToRoute(routeId, request);
            return Ok(scheduleId);
        }

        [HttpPut("{scheduleId}")]
        public async Task<IActionResult> UpdateSchedule( int scheduleId, [FromBody] UpdateScheduleDto request)
        {
          await _scheduleService.UpdateSchedule(scheduleId, request);
            return NoContent();
        }

        [HttpDelete("{scheduleId}")]
        public async Task<IActionResult> DeleteSchedule(int routeId, int scheduleId)
        {
            await _scheduleService.DeleteSchedule(routeId, scheduleId);
            return NoContent();
        }

    }
}
