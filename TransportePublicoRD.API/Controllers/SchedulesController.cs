using Microsoft.AspNetCore.Mvc;
using TransportePublicoRD.Dto.ScheduleDto;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Data.Repositories;

namespace TransportePublicoRD.Controllers
{
    [ApiController]
    [Route("api/routes/{routeId}/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly ScheduleRepository _scheduleRepository;
        private readonly RouteRepository _routeRepository;

        public SchedulesController(ScheduleRepository scheduleRepository, RouteRepository routeRepository)
        {
            _scheduleRepository = scheduleRepository;
            _routeRepository = routeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRouteSchedules(int routeId)
        {
            var route = await _routeRepository.GetByIdAsync(routeId);
            if (route == null)
                return NotFound($"Route with ID {routeId} not found.");

            var schedules = await _scheduleRepository.GetByIdAsync(routeId);
            return Ok(schedules);
        }

        [HttpGet("{scheduleId}")]
        public async Task<IActionResult> GetSchedule(int routeId, int scheduleId)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                return NotFound($"Schedule with ID {scheduleId} not found in route {routeId}.");

            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> AddScheduleToRoute(int routeId, [FromBody] CreateScheduleDto request)
        {
            if (request == null)
                return BadRequest("Invalid schedule data.");

            var route = await _routeRepository.GetByIdAsync(routeId);
            if (route == null)
                return NotFound($"Route with ID {routeId} not found.");

            var schedule = new Schedule
            {
                PublicRouteId = routeId,
                StartTime = TimeSpan.Parse(request.StartTime),
                EndTime = TimeSpan.Parse(request.EndTime),
                FrequencyMinutes = request.FrequencyMinutes
            };

            await _scheduleRepository.AddAsync(schedule);
            return Ok(schedule);
        }

        [HttpPut("{scheduleId}")]
        public async Task<IActionResult> UpdateSchedule( int scheduleId, [FromBody] UpdateScheduleDto request)
        {
            if (request == null)
                return BadRequest("Invalid schedule data.");

            var schedule = await _scheduleRepository.GetByIdAsync( scheduleId);
            if (schedule == null)
                return NotFound($"Schedule with ID {scheduleId} no found.");

            schedule.StartTime = TimeSpan.Parse(request.StartTime);
            schedule.EndTime = TimeSpan.Parse(request.EndTime);
            schedule.FrequencyMinutes = request.FrequencyMinutes;

            await _scheduleRepository.UpdateAsync(schedule);
            return NoContent();
        }

        [HttpDelete("{scheduleId}")]
        public async Task<IActionResult> DeleteSchedule(int routeId, int scheduleId)
        {
             await _scheduleRepository.DeleteAsync(scheduleId);
            if (scheduleId == null)
                return NotFound($"Schedule with ID {scheduleId} not found in route {routeId}.");

            return NoContent();
        }

    }
}
