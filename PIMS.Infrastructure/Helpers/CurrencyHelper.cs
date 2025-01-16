using PIMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Infrastructure.Helpers
{
    public static class CurrencyHelper
    {
        // Get the exchange rate from the Enum and convert the currency
        public static decimal ConvertCurrency(decimal amount, CurrencyRate rate)
        {
            // Convert the Enum value to an exchange rate
            decimal exchangeRate = rate switch
            {
                CurrencyRate.USD_TO_EUR => 0.85m,  // Exchange rate from USD to EUR
                CurrencyRate.USD_TO_VND => 23000m,  // Exchange rate from USD to VND
                CurrencyRate.EUR_TO_USD => 1.18m,  // Exchange rate from EUR to USD
                _ => throw new Exception("Unknown currency rate")  // Throw exception if rate is unknown
            };

            // Return the amount after conversion
            return amount * exchangeRate;
        }
    }
}
