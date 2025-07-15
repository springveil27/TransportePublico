using TransportePublicoRD.Application.Dto;
using TransportePublicoRD.Domain.Entities;

namespace TransportePublicoRD.Application.Interface
{
    public interface ISchedulesService
    {
        Task<int> AddScheduleToRoute(int routeId, CreateScheduleDto request);
        Task DeleteSchedule(int routeId, int scheduleId);
        Task<List<ScheduleDto>> GetRouteSchedules(int routeId);
        Task<ScheduleDto> GetSchedule(int ScheduleId);
        Task UpdateSchedule(int scheduleId, UpdateScheduleDto request);
    }
}