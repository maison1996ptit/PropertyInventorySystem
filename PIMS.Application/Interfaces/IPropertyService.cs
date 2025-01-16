using PIMS.Application.Dtos;
using PIMS.Domain.Entities;

namespace PIMS.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<Property> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Property>> GetAllAsync(int pageNumber, int pageSize
                                        , string filter, CancellationToken cancellationToken);
        Task AddAsync(Property entity, CancellationToken cancellationToken);
        Task AddAsync(IEnumerable<Property> entities, CancellationToken cancellationToken);
        Task UpdateAsync(Property entity, CancellationToken cancellationToken);
        Task UpdateAsync(IEnumerable<Property> entities, CancellationToken cancellationToken);
        Task<IEnumerable<PropertyContactsDto>> GetDataDashboardAsync(int pageNumber, int pageSize
                                        , string filter, CancellationToken cancellationToken);
    }
}
