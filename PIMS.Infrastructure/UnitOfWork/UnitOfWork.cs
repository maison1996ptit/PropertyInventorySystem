using Microsoft.Extensions.Logging;
using PIMS.Application.Interfaces;
using PIMS.Application.UnitOfWork;
using PIMS.Domain.Entities;
using PIMS.Infrastructure.Data;
using PIMS.Infrastructure.Providers.Interface;
using PIMS.Infrastructure.Repositories;

namespace PIMS.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Các repository sẽ được khởi tạo
        private IPropertyRepository<Property> _propertyRepository;
        private IContactRepository<Contact> _contactRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly ILogger<PropertyRepository> _loggerProperty;
        private readonly ILogger<ContactRepository> _loggerContact;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IPropertyRepository<Property> PropertyRepository
        {
            get
            {
                return _propertyRepository ??= new PropertyRepository(_context, _timeProvider, _loggerProperty);
            }
        }
        public IContactRepository<Contact> contactRepository
        {
            get
            {
                return _contactRepository ??= new ContactRepository(_context, _timeProvider, _loggerContact);
            }
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
