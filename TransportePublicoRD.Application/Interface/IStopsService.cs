using TransportePublicoRD.Application.Dto;

namespace TransportePublicoRD.Application.Interface
{
    public interface IStopsService
    {
        Task<StopsDto> AddStopToRoute(int routeId, CreateStopDto request);
        Task DeleteStop(int stopId);
        Task<List<StopsDto>> GetRouteStops(int routeId);
        Task<StopsDto> GetStop(int stopId);
        Task UpdateStop(int routeId, int stopId, UpdateStopDto request);
    }
}