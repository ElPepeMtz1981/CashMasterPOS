using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMasterPOS
{
    public static class Configuration
    {
        public static string CountryCode { get; set; } = "US";

        public static void ConfigureCurrency()
        {
            switch (CountryCode.ToUpper())
            {
                case "US":
                    Currency.SetDenominations(new List<decimal>
                    {
                        100.00m, 50.00m, 20.00m, 10.00m, 5.00m, 2.00m,
                        1.00m, 0.50m, 0.25m, 0.10m, 0.05m, 0.01m
                    });
                    break;

                case "MX":
                    Currency.SetDenominations(new List<decimal>
                    {
                        100.00m, 50.00m, 20.00m, 10.00m, 5.00m, 2.00m,
                        1.00m, 0.50m, 0.20m, 0.10m, 0.05m
                    });
                    break;

                default:
                    throw new InvalidOperationException("Unsupported country code.");
            }
        }
    }
}
