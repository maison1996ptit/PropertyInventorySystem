using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PIMS.Application.Interfaces;
using PIMS.Domain.Entities;
using PIMS.Infrastructure.Data;
using PIMS.Infrastructure.Providers.Interface;

namespace PIMS.Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository<Contact>
    {
        private readonly ApplicationDbContext _context;
        private readonly ITimeProvider _timeProvider;
        private readonly ILogger<ContactRepository> _logger;


        public ContactRepository(ApplicationDbContext context
                                    , ITimeProvider timeProvider
                                    , ILogger<ContactRepository> logger)
        {
            _context = context;
            _timeProvider = timeProvider;
            _logger = logger;
        }

        public async Task AddAsync(Contact entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "The property entity cannot be null.");

            // Check if the entity already exists (example: check by name)
            var existingContact = await _context.Contacts
                .FirstOrDefaultAsync(p => p.EmailAddress == entity.EmailAddress, cancellationToken);
            if (existingContact != null)
                throw new InvalidOperationException($"A Contact with the Email '{entity.EmailAddress}' already exists.");

            try
            {
                // Add the entity to the context
                await _context.Contacts.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while fetching properties.");
                throw new Exception("An error occurred while adding the property. Please try again later.");
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            // Find the property by ID
            var contact = await _context.Contacts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (contact == null)
            {
                throw new KeyNotFoundException($"Contact was not found.");
            }

            // Remove the property from the DbContext
            _context.Contacts.Remove(contact);

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all properties from the database.");
                var contacts = await _context.Contacts.ToListAsync(cancellationToken);
                return contacts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching properties.");
                throw new Exception("Failed to fetch properties. Please try again later.");
            }
        }

        public async Task<Contact> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Contact email cannot empty.");
            }

            var contact = await _context.Contacts.FirstOrDefaultAsync(p => p.EmailAddress == email, cancellationToken);

            if (contact == null)
            {
                throw new KeyNotFoundException($"Contact with email {email} not found.");
            }

            return contact;
        }

        public async Task UpdateAsync(Contact entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The Contact entity cannot be null.");
            }

            _context.Contacts.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
