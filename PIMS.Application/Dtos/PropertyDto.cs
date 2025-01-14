using PIMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Application.Dtos
{
    public class PropertyDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid? OwnerId { get; set; }
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
