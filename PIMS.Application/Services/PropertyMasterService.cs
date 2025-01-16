using PIMS.Application.Dtos;
using PIMS.Application.Interfaces;
using PIMS.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Application.Services
{
    public class PropertyMasterService : IPropertyMasterService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PropertyMasterService( IPropertyMasterRepository<PropertyContactsDto> propertyRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PropertyContactsDto>> GetDataDashboardAsync(int pageNumber, int pageSize, string filter, CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and page size must be greater than zero.");

            return await _unitOfWork.PropertyOptionRepository.GetDataDashboardAsync(pageNumber, pageSize, filter, cancellationToken);
        }
    }
}
