using TransportePublicoRD.Domain.Entities;

namespace TransportePublicoRD.Infrastructure.Interface
{
    public interface IRouteRepository : IRepository<PublicRoutes>
    {
        Task<List<PublicRoutes>> GetAllWithDetailsAsync();
        Task<PublicRoutes> GetByIdWithDetailsAsync(int id);

    }

}