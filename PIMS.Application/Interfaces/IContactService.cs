using PIMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Application.Interfaces
{
    public interface IContactService
    {
        Task<Contact> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Contact>> GetAllAsync(int pageNumber, int pageSize
                                        , string filter, CancellationToken cancellationToken);
        Task AddAsync(Contact entity, CancellationToken cancellationToken);
        Task AddAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken);
        Task UpdateAsync(Contact entity, CancellationToken cancellationToken);
        Task UpdateAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken);
    }
}
