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
            // Validate that the input entity is not null
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
            // Ensure the entity has a unique ID
            if (entity.Id == Guid.Empty)
            {
                // Assign a new GUID if the current ID is empty
                entity.Id = Guid.NewGuid();
            }
            // Add the entity to the database context
            await _context.Contacts.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken)
        {
            // Validate that the input collection is not null or empty
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException("Entities cannot be null or empty", nameof(entities));
            }
            // Ensure each entity has a unique ID
            foreach (var entity in entities)
            {
                // Assign a new GUID if the current ID is empty
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }
            }
            // Add the collection of entities to the database context
            await _context.Contacts.AddRangeAsync(entities, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync(int pageNumber, int pageSize, string filter, CancellationToken cancellationToken)
        {
            // Validate input arguments
            if (pageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.", nameof(pageNumber));
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
            }
            // Start building the query to fetch data from the database
            IQueryable<Contact> query = _context.Contacts.AsQueryable();

            // Apply filter if provided
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.FirstName.Contains(filter));
            }
            query = query
                .Skip((pageNumber - 1) * pageSize) // Skip the items of previous pages
                .Take(pageSize);                  // Take only the items for the current page

            var result = await query.ToListAsync(cancellationToken);

            return result;
        }

        public async Task<Contact> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                // Validate the input ID
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("ID cannot be an empty GUID.", nameof(id));
                }

                // Fetch the entity from the database based on the provided ID
                Contact query = await _context.Contacts.AsQueryable().Where(p=>p.Id==id )
                    .FirstOrDefaultAsync(cancellationToken);

                // Check if the entity was not found and throw an exception if needed
                if (query == null)
                {
                    throw new KeyNotFoundException($"No entity found with ID {id}.");
                }

                return query;
            }
            catch (Exception ex)
            {
                // Log or handle the exception here to see what's going wrong
                throw new InvalidOperationException("Error retrieving entity", ex);
            }
        }

        public async Task UpdateAsync(Contact entity, CancellationToken cancellationToken)
        {
            // Validate the input entity
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
            // Ensure the entity has a valid ID
            if (entity.Id == Guid.Empty)
            {
                throw new ArgumentException("Entity must have a valid ID.", nameof(entity));
            }
            // Check if the entity exists in the database

            var existingEntity = GetByIdAsync(entity.Id, cancellationToken);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"No entity found with ID {entity.Id}.");
            }

            // Update the existing entity with the new values
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            // Mark the entity as modified to ensure it's updated
            _context.Entry(existingEntity).State = EntityState.Modified;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken)
        {
            // Validate the input collection
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException("Entities cannot be null or empty.", nameof(entities));
            }

            // Iterate through the entities to perform validation and update logic
            foreach (var entity in entities)
            {
                // Ensure the entity is not null
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "One of the entities is null.");
                }

                // Ensure the entity has a valid ID
                if (entity.Id == Guid.Empty)
                {
                    throw new ArgumentException("One of the entities has an invalid ID (empty GUID).", nameof(entity));
                }

                // Check if the entity exists in the database
                var existingEntity = await _context.Contacts
                    .FirstOrDefaultAsync(p => p.Id == entity.Id, cancellationToken);

                if (existingEntity == null)
                {
                    throw new KeyNotFoundException($"No entity found with ID {entity.Id}.");
                }
                // Update the existing entity with the new values
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
