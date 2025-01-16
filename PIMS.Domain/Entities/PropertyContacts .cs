using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Domain.Entities
{
    public class PriceOfAcquisition
    {

        public PriceOfAcquisition(Guid propertyId
                                    , Guid contactId
                                    , decimal price
                                    , DateTime effectiveFrom
                                    , DateTime effectiveTill)
        {
            PropertyId = propertyId;
            ContactId = contactId;
            Price = price;
            EffectiveFrom = effectiveFrom;
            EffectiveTill = effectiveTill;
        }
        public Guid Id { get; set; }
        public PriceOfAcquisition() { }
        public Guid PropertyId { get; set; }
        public Guid ContactId { get; set; }
        public decimal Price { get; set; }
        public decimal AskingPrice { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTill { get; set; }
        public Property Property { get; set; }
        public Contact Contact { get; set; }
    }
}
