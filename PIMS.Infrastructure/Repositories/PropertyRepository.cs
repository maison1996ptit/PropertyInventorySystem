using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PIMS.Application.Interfaces;
using PIMS.Domain.Entities;
using PIMS.Infrastructure.Data;
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
        public async Task AddAsync(Property entity,CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "The property entity cannot be null.");

            // Check if the entity already exists (example: check by name)
            var existingProperty = await _context.Properties
                .FirstOrDefaultAsync(p => p.Name == entity.Name && p.Price == entity.Price,cancellationToken);
            if (existingProperty != null)
                throw new InvalidOperationException($"A property with the name '{entity.Name}' already exists.");

            // Set created date
            entity.CreatedDate = _timeProvider.Now;

            try
            {
                // Add the entity to the context
                await _context.Properties.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while fetching properties.");
                throw new Exception("An error occurred while adding the property. Please try again later.");
            }
        }

        public async Task DeleteAsync(string name, decimal price,CancellationToken cancellationToken)
        {
            // Find the property by ID
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.Name == name && p.Price == price,cancellationToken); 
            if (property == null)
            {
                throw new KeyNotFoundException($"Property with Name {name} and Price {price} was not found.");
            }

            // Remove the property from the DbContext
            _context.Properties.Remove(property);

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Property>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all properties from the database.");
                var properties = await _context.Properties.ToListAsync(cancellationToken);
                return properties;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching properties.");
                throw new Exception("Failed to fetch properties. Please try again later.");
            }
        }

        public async Task<Property> GetByNameAndPriceAsync(string name, decimal price,CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name) || price < 0)
            {
                throw new ArgumentException("Property Name Or Price cannot be null or empty.");
            }
            
            return await _context.Properties.FirstOrDefaultAsync(p => p.Name == name && p.Price == price, cancellationToken);

        }

        public async Task<IEnumerable<Property>> GetByNameAsync(string name,CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Property name cannot be null or empty.", nameof(name));
            }

            return await _context.Properties.Where(p => p.Name == name).ToListAsync(cancellationToken);

        }

        public async Task UpdateAsync(Property entity,CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The property entity cannot be null.");
            }

            _context.Properties.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
