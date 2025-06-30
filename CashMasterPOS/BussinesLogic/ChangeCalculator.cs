using CashMasterPOS.Exceptions;

namespace CashMasterPOS.BussinesLogic
{
    public static class ChangeCalculator
    {
        public static Dictionary<decimal, int> CalculateChange(decimal price, Dictionary<decimal, int> customerTender)
        {
            if (price <= 0)
            {
                throw new ChangeCalculationException(price, 0, "Price must be greater than zero.");
            }

            if (customerTender is null || customerTender.Count == 0)
            {
                throw new ChangeCalculationException(price, 0, "Tender cannot be null or empty.");
            }

            decimal totalPaid = customerTender.Sum(kayValuePair => kayValuePair.Key * kayValuePair.Value);

            if (totalPaid < price)
            {
                throw new ChangeCalculationException(price, totalPaid, $"Insufficient payment. Paid {totalPaid:C}, required {price:C}.");
            }

            decimal changeDue = Math.Round(totalPaid - price, 2);

            if (changeDue == 0)
            {
                return new Dictionary<decimal, int>();
            }

            if (Currency.Denominations == null || Currency.Denominations.Count == 0)
            {
                throw new ChangeCalculationException(price, totalPaid, "Currency denominations not configured.");
            }

            var changeResult = new Dictionary<decimal, int>();

            foreach (var denomination in Currency.Denominations)
            {
                int count = (int)(changeDue / denomination);

                if (count > 0)
                {
                    changeResult[denomination] = count;
                    changeDue -= count * denomination;
                    changeDue = Math.Round(changeDue, 2);
                }

                if (changeDue == 0)
                {
                    break;
                }
            }

            if (changeDue != 0)
            {
                throw new ChangeCalculationException(price, totalPaid, $"Exact change cannot be returned. Remainder: {changeDue:C}");
            }

            return changeResult;
        }
    }
}
