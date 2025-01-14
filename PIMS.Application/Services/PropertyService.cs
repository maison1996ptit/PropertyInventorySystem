using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PIMS.Application.Interfaces;
using PIMS.Application.UnitOfWork;
using PIMS.Application.Validators;
using PIMS.Domain.Entities;

namespace PIMS.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly PropertyValidator _propertyValidator;
        private readonly IUnitOfWork _unitOfWork;

        public PropertyService(IPropertyRepository<Property> propertyRepository, IUnitOfWork unitOfWork)
        {
            _propertyValidator = new PropertyValidator();
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Property entity,CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = _propertyValidator.Validate(entity);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException("Property validation failed.", validationResult.Errors);
                }

                await _unitOfWork.PropertyRepository.AddAsync(entity,cancellationToken);
            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while adding the property.", ex);
            }
        }

        public async Task DeleteAsync(string name,decimal price, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name) || price < 0)
            {
                throw new ArgumentException("The Name or Price cannot be an empty.");
            }

            // Find Property is ready exits in database
            var property = await _unitOfWork.PropertyRepository.GetByNameAsync(name,cancellationToken);
            if (property == null)
            {
                throw new KeyNotFoundException($"Property with Name is {name} and Price: {price} does not exist.");
            }
            // Deleted record
            try
            {
                await _unitOfWork.PropertyRepository.DeleteAsync(name,price,cancellationToken);
            }
            catch (Exception ex) when (ex is not KeyNotFoundException || ex is not ArgumentException)
            {
                throw new Exception("Failed to delete the property. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<Property>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.PropertyRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Property> GetByNameAndPriceAsync(string name, decimal price, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name) || price < 0)
                throw new ArgumentException("Property Name or Price cannot be null or empty.", nameof(name));

            try
            {
                var property = await _unitOfWork.PropertyRepository.GetByNameAndPriceAsync(name, price, cancellationToken);

                if (property == null)
                    throw new KeyNotFoundException($"Property with Name '{name}' and Price {price} does not exist.");

                return property;
            }
            catch (ArgumentException)
            {
                // Re-throw input validation issues
                throw;
            }
            catch (KeyNotFoundException)
            {
                // Re-throw not found exceptions
                throw;
            }
            catch (Exception ex)
            {
                // Log detailed error (using your logging mechanism)
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");

                // Provide meaningful error message to the consumer
                throw new Exception("An unexpected error occurred while processing the request.", ex);
            }
        }


        public async Task<IEnumerable<Property>> GetByNameAsync(string name,CancellationToken cancellationToken)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Property name cannot be null or empty.", nameof(name));

            try
            {
                // Call repository
                var property = await _unitOfWork.PropertyRepository.GetByNameAsync(name,cancellationToken);

                // Handle not found case
                if (property == null)
                {
                    // Optionally throw an exception or return null
                    throw new KeyNotFoundException($"Property with name '{name}' does not exist.");
                }

                return property;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException || ex is not ArgumentException)
            {
                // Only handle unexpected errors
                throw new Exception("An error occurred while finding the property.", ex);
            }
        }

        public async Task UpdateAsync(Property entity,CancellationToken cancellationToken)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "The property entity cannot be null.");

            // Find Property is ready exits in database
            var existingEntity = await _unitOfWork.PropertyRepository.GetByNameAndPriceAsync(entity.Name
                                                                                ,entity.Price
                                                                                ,cancellationToken);
            if (existingEntity != null)
                throw new KeyNotFoundException($"Property with Name {entity.Name} is exits found.");

            // Update record
            try
            {
                await _unitOfWork.PropertyRepository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex) when (ex is not KeyNotFoundException || ex is not ArgumentException)
            {
                // Only handle unexpected errors
                throw new Exception("An error occurred while Update the property.", ex);
            }
        }
    }
}
