using PIMS.Application.Dtos;
using PIMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Application.Mappings
{
    public static class PropertyMapper
    {
        public static IEnumerable<Property> ToEntities(IEnumerable<PropertyDto> dtos)
        {
            return dtos.Select(dto => new Property
            {
                Name = dto.Name,
                Price = dto.Price,
                Address = dto.Address,
            });
        }
        public static IEnumerable<Property> ToEntities(IEnumerable<UpdatePropertyDto> dtos)
        {
            return dtos.Select(dto => new Property
            {
                Name = dto.Name,
                Price = dto.Price,
                Address = dto.Address,
                Id = dto.Id,
            });
        }
    }
}
