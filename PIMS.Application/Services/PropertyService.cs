using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PIMS.Application.Dtos;
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

        public async Task AddAsync(Property entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var validationResult = _propertyValidator.Validate(entity);

            if (!validationResult.IsValid)
                throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            await _unitOfWork.PropertyRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(IEnumerable<Property> entities, CancellationToken cancellationToken)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("Entities collection is null or empty.");

            foreach (var entity in entities)
            {
                var validationResult = _propertyValidator.Validate(entity);
                if (!validationResult.IsValid)
                    throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            await _unitOfWork.PropertyRepository.AddAsync(entities, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Property>> GetAllAsync(int pageNumber, int pageSize, string filter, CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and page size must be greater than zero.");

            return await _unitOfWork.PropertyRepository.GetAllAsync( pageNumber, pageSize,filter, cancellationToken);
        }

        public async Task<Property> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be an empty GUID.");

            var property = await _unitOfWork.PropertyRepository.GetByIdAsync(id, cancellationToken);
            if (property == null)
                throw new KeyNotFoundException($"Property with ID {id} not found.");

            return property;
        }

        public async Task UpdateAsync(Property entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var validationResult = _propertyValidator.Validate(entity);
            if (!validationResult.IsValid)
                throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            await _unitOfWork.PropertyRepository.UpdateAsync(entity,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(IEnumerable<Property> entities, CancellationToken cancellationToken)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("Entities collection is null or empty.");

            foreach (var entity in entities)
            {
                var validationResult = _propertyValidator.Validate(entity);
                if (!validationResult.IsValid)
                    throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            await _unitOfWork.PropertyRepository.UpdateAsync(entities,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<IEnumerable<PropertyContactsDto>> GetDataDashboardAsync(int pageNumber, int pageSize, string filter, CancellationToken cancellationToken)
        {
            return await _unitOfWork.PropertyRepository.GetDataDashboardAsync(pageNumber, pageSize, filter, cancellationToken);
        }
    }
}
