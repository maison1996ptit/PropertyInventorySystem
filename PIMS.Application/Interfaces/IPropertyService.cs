using PIMS.Domain.Entities;

namespace PIMS.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<IEnumerable<Property>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Property entity, CancellationToken cancellationToken);
        Task UpdateAsync(Property entity, CancellationToken cancellationToken);
        Task DeleteAsync(string name, decimal price, CancellationToken cancellationToken);
        Task<Property> GetByNameAndPriceAsync(string name, decimal price, CancellationToken cancellationToken);
    }
}
