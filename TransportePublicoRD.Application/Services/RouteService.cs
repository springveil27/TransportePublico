using TransportePublicoRD.Domain.Entities;
using TransportePublicoRD.Infrastructure.Interface;
using TransportePublicoRD.Application.Dto;
using TransportePublicoRD.Application.Interface;


namespace TransportePublicoRD.Application.Services
{
    public class RouteService : IRouteService
    {
        // create controller to manage public routes
        private readonly IUnitOfWork _unitOfWork;

        public RouteService(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        // function to get all routes

        public async Task<List<RouteDto>> GetRoutes()
        {
            var routes = await _unitOfWork.RouteRepository.GetAllAsync();
            if (routes == null || !routes.Any())
            {
                throw new Exception("No routes found.");
            }
            var routesWithDetails = routes.Select
                (r => new RouteDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Code = r.Code,
                    Cost = r.Cost,
                    Active = r.Active,
                    Stops = r.Stops.Select(s => new StopsDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Order = s.Order
                    }).ToList(),
                    Schedules = r.Schedules.Select(s => new ScheduleDto
                    {
                        Id = s.Id,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        FrequencyMinutes = s.FrequencyMinutes
                    }).ToList()
                }).ToList();
            return routesWithDetails;
        }


        public async Task<RouteDto> GetRouteById(int Id)
        {
            var route = await _unitOfWork.RouteRepository.GetByIdAsync(Id);

            if (route == null)
            {
                throw new Exception($"Route with ID {Id} not found.");
            }
            var routeTrans = new RouteDto
            {
                Id = route.Id,
                Name = route.Name,
                Code = route.Code,
                Cost = route.Cost,
                Active = route.Active,

            };
            return routeTrans;
        }


        public async Task<int> CreateRoute(CreatePublicRouteDto request)
        {
            if (request == null)
            {
                throw new Exception("Invalid route data.");
            }

            var route = new PublicRoutes
            {
                Name = request.Name,
                Code = request.Code,
                Cost = request.Cost,
                Active = request.Active,
                CreatedDate = DateTime.Now
            };
            await _unitOfWork.RouteRepository.AddAsync(route);
            await _unitOfWork.SaveAsync();
            return route.Id;
        }


        public async Task UpdateRoute( UpdatePublicRouteDto request)
        {
            if (request == null)
            {
                throw new Exception("Invalid route data.");
            }
            var existingRoute = await _unitOfWork.RouteRepository.GetByIdAsync(request.Id);
            if (existingRoute == null)
            {
                throw new Exception($"Route with ID {request.Id} not found.");
            }
            existingRoute.Id = request.Id;
            existingRoute.Name = request.Name;
            existingRoute.Code = request.Code;
            existingRoute.Cost = request.Cost;
            existingRoute.UpdatedDate = DateTime.Now;
            existingRoute.Active = request.Active;
            await _unitOfWork.RouteRepository.UpdateAsync(existingRoute);
            await _unitOfWork.SaveAsync();

        }

        public async Task DeleteRoute(int Id)
        {
            var routes = await _unitOfWork.RouteRepository.GetByIdAsync(Id);
            if (routes == null)
            {
                throw new Exception($"Route with ID {Id} not found.");
            }
            await _unitOfWork.RouteRepository.DeleteAsync(Id);
            await _unitOfWork.SaveAsync();

        }

    }
}
