using PIMS.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Application.Interfaces
{
    public interface IPropertyMasterService
    {
        Task<IEnumerable<PropertyContactsDto>> GetDataDashboardAsync(int pageNumber, int pageSize
                                       , string filter, CancellationToken cancellationToken);
    }
}
