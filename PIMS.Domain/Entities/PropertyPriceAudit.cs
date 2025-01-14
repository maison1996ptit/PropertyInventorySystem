using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Domain.Entities
{
    public  class PropertyPriceAudit
    {
        public PropertyPriceAudit(Guid id, Guid propertyId, decimal price,  DateTime createdDate)
        {
            Id = id;
            PropertyId = propertyId;
            Price = price;
            CreatedDate = createdDate;
        }

        public Guid Id { get; set; }
        public PropertyPriceAudit() { }
        public Guid PropertyId { get; set; } 
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public Property Property { get; set; }
    }
}
