namespace PIMS.Application.Interfaces
{
    public interface IPropertyRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetByNameAsync(string name,CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(T entity,CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(string name,decimal price,CancellationToken cancellationToken);
        Task<T> GetByNameAndPriceAsync(string name,decimal price,CancellationToken cancellationToken);
    }
}
