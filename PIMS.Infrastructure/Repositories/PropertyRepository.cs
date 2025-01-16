using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PIMS.Application.Dtos;
using PIMS.Application.Interfaces;
using PIMS.Domain.Entities;
using PIMS.Domain.Enums;
using PIMS.Infrastructure.Data;
using PIMS.Infrastructure.Helpers;
using PIMS.Infrastructure.Providers.Interface;

namespace PIMS.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository<Property>
    {
        private readonly ApplicationDbContext _context;
        private readonly ITimeProvider _timeProvider;
        private readonly ILogger<PropertyRepository> _logger;

        public PropertyRepository(ApplicationDbContext context
                                    ,ITimeProvider timeProvider
                                    , ILogger<PropertyRepository> logger)
        {
            _context = context;
            _timeProvider = timeProvider;
            _logger = logger;
        }

        public async Task AddAsync(Property entity, CancellationToken cancellationToken)
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
            entity.CreatedDate = DateTime.UtcNow;
            // Add the entity to the database context
            await _context.Properties.AddAsync(entity, cancellationToken);
        
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(IEnumerable<Property> entities, CancellationToken cancellationToken)
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
            await _context.Properties.AddRangeAsync(entities, cancellationToken);
          
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Property>> GetAllAsync(int pageNumber, int pageSize, string filter, CancellationToken cancellationToken)
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
            IQueryable<Property> query = _context.Properties.AsQueryable();

            // Apply filter if provided
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.Name.Contains(filter));
            }
            query = query
                .Skip((pageNumber - 1) * pageSize) // Skip the items of previous pages
                .Take(pageSize);                  // Take only the items for the current page

            var result = await query.ToListAsync(cancellationToken);

            return result;
        }

        public async Task<Property> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            // Validate the input ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be an empty GUID.", nameof(id));
            }
            // Fetch the entity from the database based on the provided ID
            var entity = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            // Check if the entity was not found and throw an exception if needed
            if (entity == null)
            {
                throw new KeyNotFoundException($"No entity found with ID {id}.");
            }
            return entity;
        }

        public async Task UpdateAsync(Property entity, CancellationToken cancellationToken)
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
            var existingEntity =  _context.Properties.FirstOrDefault(p => p.Id == entity.Id);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"No entity found with ID {entity.Id}.");
            }
            // Add audit table
            await AddAudit(entity, existingEntity, cancellationToken);
            // Update the existing entity with the new values
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(IEnumerable<Property> entities, CancellationToken cancellationToken)
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
                var existingEntity =  _context.Properties.FirstOrDefault(p => p.Id == entity.Id);

                if (existingEntity == null)
                {
                    throw new KeyNotFoundException($"No entity found with ID {entity.Id}.");
                }
                // Add audit table
                await AddAudit(entity, existingEntity, cancellationToken);
                // Update the existing entity with the new values
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task AddAudit(Property newData, Property oldData, CancellationToken cancellationToken)
        {
            var auditData = new PropertyPriceAudit
            {
                PropertyId = newData.Id,
                Price = oldData.Price,
                CreatedDate = DateTime.UtcNow
            };

            await _context.PropertyPriceAudits.AddAsync(auditData, cancellationToken);
        }
        public async Task<IEnumerable<PropertyContactsDto>> GetDataDashboardAsync(int pageNumber, int pageSize
                                        , string filter, CancellationToken cancellationToken)
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
            IQueryable<PropertyContactsDto> query =
                                                    from priceOfAcquisitions in _context.PriceOfAcquisitions
                                                    join contact in _context.Contacts
                                                        on priceOfAcquisitions.ContactId equals contact.Id into contactGroup
                                                    from contact in contactGroup.DefaultIfEmpty()
                                                    join property in _context.Properties
                                                        on priceOfAcquisitions.PropertyId equals property.Id into propertyGroup
                                                    from property in propertyGroup.DefaultIfEmpty()
                                                    join audit in _context.PropertyPriceAudits
                                                        on priceOfAcquisitions.PropertyId equals audit.PropertyId into auditGroup
                                                    from audit in auditGroup.DefaultIfEmpty()
                                                    let soldAtUsd = CurrencyHelper.ConvertCurrency(property.Price
                                                                                                    , CurrencyRate.EUR_TO_USD) 
                                                    select new PropertyContactsDto
                                                    {
                                                        PropertyName = property.Name,
                                                        AskingPrice = audit.Price == 0 ? audit.Price : 0 ,
                                                        Owner = contact == null ? string.Empty 
                                                                            : contact.FirstName + " " + contact.LastName,
                                                        DatePurchare = priceOfAcquisitions.EffectiveFrom,
                                                        SoldAtEur = property.Price,
                                                        SoldAtUSD = soldAtUsd,
                                                    };
            // Apply filter if provided
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.PropertyName.Contains(filter));
            }
            query = query
                .Skip((pageNumber - 1) * pageSize) // Skip the items of previous pages
                .Take(pageSize);                  // Take only the items for the current page

            var result = await query.ToListAsync(cancellationToken);

            return result;
        }
    }
}
