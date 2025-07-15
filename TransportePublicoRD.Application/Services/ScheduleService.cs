using Microsoft.EntityFrameworkCore;
using TransportePublicoRD.Application.Dto;
using TransportePublicoRD.Application.Interface;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Interface;

namespace TransportePublicoRD.Application.Services
{
    public class SchedulesService : ISchedulesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SchedulesService( IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }


        public async Task<ScheduleDto> GetSchedule(int scheduleId)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(scheduleId);

            if (schedule == null)
                throw new Exception($"Schedule with ID {scheduleId} not found.");

            return new ScheduleDto
            {
                Id = schedule.Id,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                FrequencyMinutes = schedule.FrequencyMinutes,
                PublicRouteId = schedule.PublicRouteId
            };
        }
        public async Task<List<ScheduleDto>> GetRouteSchedules(int routeId)
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetAllAsync();
            var filteredSchedules = schedules.Where(s => s.PublicRouteId == routeId);

            return filteredSchedules.Select(s => new ScheduleDto
            {
                Id = s.Id,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                FrequencyMinutes = s.FrequencyMinutes,
                PublicRouteId = s.PublicRouteId
            }).ToList();
        }

        public async Task<int> AddScheduleToRoute(int routeId, CreateScheduleDto request)
        {
            if (request == null)
                throw new Exception("Invalid schedule data.");

            var route = await _unitOfWork.RouteRepository.GetByIdAsync(routeId);
            if (route == null)
                throw new Exception($"Route with ID {routeId} not found.");

            var schedule = new Schedule
            {
                PublicRouteId = routeId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                FrequencyMinutes = request.FrequencyMinutes
            };

            await _unitOfWork.ScheduleRepository.AddAsync(schedule);
            await _unitOfWork.SaveAsync();
            return schedule.Id;
        }

        public async Task UpdateSchedule(int scheduleId, UpdateScheduleDto request)
        {
            if (request == null)
                throw new Exception("Invalid schedule data.");

            var schedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new Exception($"Schedule with ID {scheduleId} no found.");

            schedule.StartTime = TimeSpan.Parse(request.StartTime);
            schedule.EndTime = TimeSpan.Parse(request.EndTime);
            schedule.FrequencyMinutes = request.FrequencyMinutes;

            await _unitOfWork.ScheduleRepository.UpdateAsync(schedule);
            await _unitOfWork.SaveAsync();

        }


        public async Task DeleteSchedule(int routeId, int scheduleId)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new Exception($"Schedule with ID {scheduleId} not found in route {routeId}.");

            await _unitOfWork.ScheduleRepository.DeleteAsync(scheduleId);
            await _unitOfWork.SaveAsync();

        }
    }
}
