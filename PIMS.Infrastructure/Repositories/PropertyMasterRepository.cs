using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PIMS.Application.Dtos;
using PIMS.Application.Interfaces;
using PIMS.Domain.Enums;
using PIMS.Infrastructure.Data;
using PIMS.Infrastructure.Helpers;
using PIMS.Infrastructure.Providers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Infrastructure.Repositories
{
    public class PropertyMasterRepository : IPropertyMasterRepository<PropertyContactsDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly ITimeProvider _timeProvider;
        private readonly ILogger<PropertyContactsDto> _logger;

        public PropertyMasterRepository(ApplicationDbContext context
                                    , ITimeProvider timeProvider
                                    , ILogger<PropertyContactsDto> logger)
        {
            _context = context;
            _timeProvider = timeProvider;
            _logger = logger;
        }

        public async Task<IEnumerable<PropertyContactsDto>> GetDataDashboardAsync(int pageNumber, int pageSize, string filter, CancellationToken cancellationToken)
        {
            try
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
                                                       let soldAtUsd = property != null
                                                           ? CurrencyHelper.ConvertCurrency(property.Price, CurrencyRate.EUR_TO_USD)
                                                           : 0
                                                       select new PropertyContactsDto
                                                       {
                                                           Id = priceOfAcquisitions.Id,
                                                           PropertyName = property != null ? property.Name : string.Empty,
                                                           AskingPrice = (audit != null && audit.Price != 0)
                                                               ? audit.Price
                                                               : (property != null ? property.Price : 0),
                                                           Owner = contact != null
                                                               ? contact.FirstName + " " + contact.LastName
                                                               : string.Empty,
                                                           DatePurchare = priceOfAcquisitions.EffectiveFrom,
                                                           SoldAtEur = property != null ? property.Price : 0,
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
            catch (ArgumentException ex)
            {
                // Handle validation exceptions
                // Log the exception as needed
                throw new InvalidOperationException("Invalid input parameters.", ex);
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
                throw;
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                // Log the exception and rethrow or return an empty list
                // Example: _logger.LogError(ex, "An error occurred while fetching dashboard data.");
                throw new InvalidOperationException("An error occurred while fetching data.", ex);
            }
        }
    } 
}
