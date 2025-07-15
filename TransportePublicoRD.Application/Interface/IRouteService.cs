using TransportePublicoRD.Application.Dto;

namespace TransportePublicoRD.Application.Interface
{
    public interface IRouteService
    {
        Task<int> CreateRoute(CreatePublicRouteDto request);
        Task DeleteRoute(int Id);
        Task<RouteDto> GetRouteById(int Id);
        Task<List<RouteDto>> GetRoutes();
        Task UpdateRoute( UpdatePublicRouteDto request);
    }
}