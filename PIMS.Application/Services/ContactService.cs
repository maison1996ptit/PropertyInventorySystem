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
            try
            {
                var validationResult = _contactValidator.Validate(entity);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException("Contact validation failed.", validationResult.Errors);
                }

                await _unitOfWork.contactRepository.AddAsync(entity, cancellationToken);
            }
            catch (Exception ex) when (ex is not ValidationException )
            {
                throw new Exception("An error occurred while adding the Contact.", ex);
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("The ID cannot be empty.");
            }
            try
            {
                await _unitOfWork.contactRepository.DeleteAsync(id, cancellationToken);
            }
            catch (Exception ex) when ( ex is not ArgumentException)
            {
                throw new Exception("Failed to delete the Contact. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken)
        {

            return await _unitOfWork.contactRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Contact> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }
            try
            {
                return await _unitOfWork.contactRepository.GetByEmailAsync(email,cancellationToken);
            }
            catch (Exception ex) when ( ex is not ArgumentException)
            {
                throw new Exception("An error occurred while Find Contact.", ex);
            }
        }

        public async Task UpdateAsync(Contact entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "The property entity cannot be null.");
            try
            {
                await _unitOfWork.contactRepository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex) when (ex is not KeyNotFoundException || ex is not ArgumentException)
            {
                throw new Exception("An error occurred while Update the property.", ex);
            }
        }
    }
}
