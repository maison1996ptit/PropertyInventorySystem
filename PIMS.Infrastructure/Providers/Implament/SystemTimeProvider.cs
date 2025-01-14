using PIMS.Infrastructure.Providers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Infrastructure.Providers.Implament
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
