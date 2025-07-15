
using TransportePublicoRD.Application.Dto;
using TransportePublicoRD.Application.Interface;
using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Interface;

namespace TransportePublicoRD.Application.Services
{
    public class StopsService : IStopsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StopsService(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }


        public async Task<List<StopsDto>> GetRouteStops(int routeId)
        {
            var stops = await _unitOfWork.StopRepository.GetAllAsync();
            var stopsWithRoute = stops.Where(s => s.PublicRouteId == routeId);
            if (stops == null)
                throw new Exception($"Route with ID {routeId} not found.");

            return stopsWithRoute.Select(s => new StopsDto
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                Order = s.Order,
                PublicRouteId = s.PublicRouteId,
            }).ToList();
           
        }


        public async Task<StopsDto> GetStop(int stopId)
        {
            var stop = await _unitOfWork.StopRepository.GetByIdAsync(stopId);
            if (stop == null)
                throw new Exception($"Stop with ID {stopId} not found.");

            var stopid = new StopsDto
            {
                Id = stop.Id,
                Name = stop.Name,
                Address = stop.Address,
                Order = stop.Order,
                PublicRouteId = stop.PublicRouteId,

            };
            return stopid;
        }


        public async Task<StopsDto> AddStopToRoute(int routeId, CreateStopDto request)
        {
            if (request == null)
                throw new Exception("Invalid stop data.");

            var route = await _unitOfWork.RouteRepository.GetByIdAsync(routeId);
            if (route == null)
                throw new Exception($"Route with ID {routeId} not found.");

            var stop = new Stop
            {
                Name = request.Name,
                Address = request.Address,
                Order = request.Order,
                PublicRouteId = routeId
            };

            await _unitOfWork.StopRepository.AddAsync(stop);
            await _unitOfWork.SaveAsync();

            var stopDto = new StopsDto
            {
                Id = stop.Id,
                Name = stop.Name,
                Address = stop.Address,
                Order = stop.Order,
                PublicRouteId = stop.PublicRouteId
            };

            return stopDto;
        }


        public async Task UpdateStop(int routeId, int stopId, UpdateStopDto request)
        {
            if (request == null)
                throw new Exception("Invalid stop data.");

            var stop = await _unitOfWork.StopRepository.GetByIdAsync(stopId);
            if (stop == null)
                throw new Exception($"Stop with ID {stopId} not found in route {routeId}.");

            stop.Name = request.Name;
            stop.Address = request.Address;
            stop.Order = request.Order;

            await _unitOfWork.StopRepository.UpdateAsync(stop);
            await _unitOfWork.SaveAsync();

        }


        public async Task DeleteStop(int stopId)
        {
            await _unitOfWork.StopRepository.DeleteAsync(stopId);
            if (stopId == null)
                throw new Exception($"Stop with ID {stopId} not found.");
            await _unitOfWork.SaveAsync();
        }
    }
}
