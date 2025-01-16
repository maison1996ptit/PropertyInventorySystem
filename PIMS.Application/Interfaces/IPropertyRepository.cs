using PIMS.Application.Dtos;

namespace PIMS.Application.Interfaces
{
    public interface IPropertyRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize
                                        , string filter, CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    }
}
