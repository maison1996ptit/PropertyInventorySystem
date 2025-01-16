using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PIMS.Application.Interfaces;
using PIMS.Application.UnitOfWork;
using PIMS.Application.Validators;
using PIMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PIMS.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactValidator _contactValidator;
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IContactRepository<Contact> contactRepository, IUnitOfWork unitOfWork)
        {
            _contactValidator = new ContactValidator();
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Contact entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var validationResult = _contactValidator.Validate(entity);
            if (!validationResult.IsValid)
                throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            await _unitOfWork.contactRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("Entities collection is null or empty.");

            foreach (var entity in entities)
            {
                var validationResult = _contactValidator.Validate(entity);
                if (!validationResult.IsValid)
                    throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            await _unitOfWork.contactRepository.AddAsync(entities, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync(int pageNumber, int pageSize, string filter, CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and page size must be greater than zero.");

            return await _unitOfWork.contactRepository.GetAllAsync(pageNumber, pageSize, filter, cancellationToken);
        }

        public async Task<Contact> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be an empty GUID.");

            var property = await _unitOfWork.contactRepository.GetByIdAsync(id, cancellationToken);
            if (property == null)
                throw new KeyNotFoundException($"Property with ID {id} not found.");

            return property;
        }

        public async Task UpdateAsync(Contact entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var validationResult = _contactValidator.Validate(entity);
            if (!validationResult.IsValid)
                throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            _unitOfWork.contactRepository.UpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("Entities collection is null or empty.");

            foreach (var entity in entities)
            {
                var validationResult = _contactValidator.Validate(entity);
                if (!validationResult.IsValid)
                    throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            _unitOfWork.contactRepository.UpdateAsync(entities, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
