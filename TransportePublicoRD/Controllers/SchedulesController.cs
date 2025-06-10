using Microsoft.AspNetCore.Mvc;
using TransportePublicoRD.Data;
using TransportePublicoRD.Dto.ScheduleDto;
using TransportePublicoRD.Entities;

namespace TransportePublicoRD.Controllers
 
{
    [ApiController]
    [Route("api/routes/{routeId}/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly DbContextApp _context;

        public SchedulesController(DbContextApp context)
        {
            _context = context;
        }

        [HttpGet]
       public IActionResult GetRouteSchedules(int routeId)
        {
            var route = _context.PublicRoutes.FirstOrDefault(r => r.Id == routeId);
            if(route == null)
            {
                return NotFound($"Route with ID {routeId} not found.");
            }
            var schedules = _context.Schedules
                 .Where(s => s.PublicRouteId == routeId)
                 .OrderBy(s => s.DayOfWeek)
                 .ThenBy(s => s.StartTime)
                 .ToList();

            return Ok(schedules);
        }

        [HttpGet("{scheduleId}")]
        public IActionResult GetSchedule(int routeId, int scheduleId)
        {
            var schedule = _context.Schedules
                .FirstOrDefault(s => s.Id == scheduleId && s.PublicRouteId == routeId);

            if (schedule == null)
            {
                return NotFound($"Schedule with ID {scheduleId} not found in route {routeId}.");
            }

            return Ok(schedule);
        }

        [HttpPost]
        public IActionResult AddScheduleToRoute(int routeId, [FromBody] CreateScheduleDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid schedule data.");
            }

            var route = _context.PublicRoutes.FirstOrDefault(r => r.Id == routeId);
            if (route == null)
            {
                return NotFound($"Route with ID {routeId} not found.");
            }

            var schedule = new Schedule
            {
                PublicRouteId = routeId,
                DayOfWeek = request.DayOfWeek,
                StartTime = TimeSpan.Parse(request.StartTime),
                EndTime = TimeSpan.Parse(request.EndTime),
                FrequencyMinutes = request.FrequencyMinutes
            };

            _context.Schedules.Add(schedule);
            _context.SaveChanges();

            return Ok(schedule);
        }

        [HttpPut("{scheduleId}")]
        public IActionResult UpdateSchedule(int routeId, int scheduleId, [FromBody] UpdateScheduleDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid schedule data.");
            }

            var schedule = _context.Schedules
                .FirstOrDefault(s => s.Id == scheduleId && s.PublicRouteId == routeId);

            if (schedule == null)
            {
                return NotFound($"Schedule with ID {scheduleId} not found in route {routeId}.");
            }

            schedule.DayOfWeek = request.DayOfWeek;
            schedule.StartTime = TimeSpan.Parse(request.StartTime);
            schedule.EndTime = TimeSpan.Parse(request.EndTime);
            schedule.FrequencyMinutes = request.FrequencyMinutes;

            _context.Update(schedule);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{scheduleId}")]
        public IActionResult DeleteSchedule(int routeId, int scheduleId)
        {
            var schedule = _context.Schedules
                .FirstOrDefault(s => s.Id == scheduleId && s.PublicRouteId == routeId);

            if (schedule == null)
            {
                return NotFound($"Schedule with ID {scheduleId} not found in route {routeId}.");
            }

            _context.Remove(schedule);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("by-day/{dayOfWeek}")]
        public IActionResult GetSchedulesByDay(int routeId, DayOfWeek dayOfWeek)
        {
            var route = _context.PublicRoutes.FirstOrDefault(r => r.Id == routeId);
            if (route == null)
            {
                return NotFound($"Route with ID {routeId} not found.");
            }

            var schedules = _context.Schedules
                .Where(s => s.PublicRouteId == routeId && s.DayOfWeek == dayOfWeek)
                .OrderBy(s => s.StartTime)
                .ToList();

            return Ok(schedules);
        }
    }
}