namespace TransportePublicoRD.Infrastructure.Interface
{
    public interface IUnitOfWork
    {
        IRouteRepository RouteRepository { get; }
        IScheduleRepository ScheduleRepository { get; }
        IStopRepository StopRepository { get; }

        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}