using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Application.Dtos
{
    public class PropertyContactsDto 
    {
        public string PropertyName { get; set; }
        public decimal AskingPrice { get; set; }
        public string Owner { get; set; }
        public DateTime DatePurchare { get;set; }
        public decimal SoldAtEur { get; set; }
        public decimal SoldAtUSD { get; set;}
    }
}
