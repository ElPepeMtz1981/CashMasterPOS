using CashMasterPOS.BussinesLogic;
using CashMasterPOS.Exceptions;

namespace CashMasterPOS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("***** POS Change Calculator *****");
            Console.Write("Select country (US/MX):");
            var country = Console.ReadLine()?.Trim().ToUpper();
            Console.WriteLine();

            try
            {
                Configuration.CountryCode = country;
                Configuration.ConfigureCurrency();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Configuration error: {ex.Message}");
                return;
            }

            bool goOn = true;

            while (goOn)
            {
                Console.Clear();
                Console.WriteLine("***** POS Change Calculator *****");

                Console.WriteLine("\nNew operation.");
                Console.WriteLine();

                Console.Write("Total to pay: ");
                var strPrice = Console.ReadLine();

                if (!decimal.TryParse(strPrice, NumberStyles.Number, CultureInfo.CurrentCulture, out var precio) || precio <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    continue;
                }

                Console.WriteLine();
                Console.Write("How many denominations? ");
                var strQtyDenominations = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(strQtyDenominations, out var totalDenominations) || totalDenominations <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    continue;
                }

                var paymentDenominationsAndQTY = new Dictionary<decimal, int>();

                for (int i = 1; i <= totalDenominations; i++)
                {
                    Console.Write($"Denomination value #{i}: ");
                    var strValue = Console.ReadLine();

                    Console.Write("Quantity: ");
                    var strQty = Console.ReadLine();

                    if (!decimal.TryParse(strValue, NumberStyles.Number, CultureInfo.CurrentCulture, out var valor) ||
                        !int.TryParse(strQty, out var qty) || qty <= 0)
                    {
                        Console.WriteLine("Invalid input, this input is going to be ignored");
                        continue;
                    }

                    if (paymentDenominationsAndQTY.ContainsKey(valor))
                    {
                        paymentDenominationsAndQTY[valor] += qty;
                    }
                    else
                    {
                        paymentDenominationsAndQTY[valor] = qty;
                    }
                }

                try
                {
                    if (!TenderValidator.TryValidate(paymentDenominationsAndQTY, Currency.Denominations, out var validationError))
                    {
                        Console.WriteLine($"Validation failed: {validationError}");
                        Console.WriteLine($"Press eny key to continue...");
                        Console.ReadKey();
                        continue;
                    }

                    var changeDenominationAndQty = ChangeCalculator.CalculateChange(precio, paymentDenominationsAndQTY);
                    Console.WriteLine();
                    Console.WriteLine("Change:");

                    foreach (var item in changeDenominationAndQty)
                    {
                        Console.WriteLine($"   {item.Key,6:C2} x {item.Value}");
                    }
                }
                catch (ChangeCalculationException ex)
                {
                    Console.WriteLine($"Error:: {ex.Message}");
                    Console.WriteLine($"Payment: {ex.Payment:C2} | Price: {ex.Price:C2}");
                }

                Console.WriteLine();
                Console.WriteLine("Continue? (Y/N):");
                var response = Console.ReadLine()?.Trim().ToUpper();

                if (response != "Y")
                {
                    goOn = false;
                }
            }

            Console.WriteLine("CASH Masters POS.\nBye.");
        }
    }
}
