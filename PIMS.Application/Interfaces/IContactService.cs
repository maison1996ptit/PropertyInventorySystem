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
        Task<Contact> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Contact entity, CancellationToken cancellationToken);
        Task UpdateAsync(Contact entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
