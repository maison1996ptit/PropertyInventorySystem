using PIMS.Application.Dtos;
using PIMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Application.Mappings
{
    public static class ContactMapper
    {
        public static IEnumerable<Contact> ToEntities(IEnumerable<ContactDto> dtos)
        {
            return dtos.Select(dto => new Contact
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                PhoneNumber = dto.PhoneNumber,
            });
        }
        public static IEnumerable<Contact> ToEntities(IEnumerable<UpdateContactDto> dtos)
        {
            return dtos.Select(dto => new Contact
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                PhoneNumber = dto.PhoneNumber,
                Id = dto.Id,
            });
        }
    }
}
