using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Domain.Entities
{
    public class PropertyContacts
    {

        public PropertyContacts(Guid propertyId
                                    , Guid contactId
                                    , decimal price
                                    , DateTime effectiveFrom
                                    , DateTime effectiveTill,
DateTime createdDate)
        {
            PropertyId = propertyId;
            ContactId = contactId;
            PriceOfAcquisition = price;
            EffectiveFrom = effectiveFrom;
            EffectiveTill = effectiveTill;
            CreatedDate = createdDate;
        }
        public Guid Id { get; set; }
        public PropertyContacts() { }
        public Guid PropertyId { get; set; }
        public Guid ContactId { get; set; }
        public decimal PriceOfAcquisition { get; set; }
        public decimal AskingPrice { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EffectiveTill { get; set; }
        public Property Property { get; set; }
        public Contact Contact { get; set; }
    }
}
