using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMasterPOS.Exceptions
{
    public class ChangeCalculationException : Exception
    {
        public decimal Payment { get; }

        public decimal Price { get; }

        public ChangeCalculationException(decimal price, decimal payment, string message)
            : base(message)
        {
            Payment = payment;
            Price = price;
        }

        public ChangeCalculationException(decimal price, decimal payment)
            : this(price, payment, $"Pay: {payment:C}, Price: {price:C}.")
        {
        }
    }
}
