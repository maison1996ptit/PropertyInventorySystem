using PIMS.Application.Interfaces;
using PIMS.Domain.Entities;

namespace PIMS.Application.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IPropertyRepository<Property> PropertyRepository { get; }
        IContactRepository<Contact> contactRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
