namespace CashMasterPOS.BussinesLogic
{
    public static class Currency
    {
        public static List<decimal> Denominations { get; set; }

        public static void SetDenominations(List<decimal> denominations)
        {
            if (denominations == null || denominations.Count == 0)
            {
                throw new ArgumentException("Denominations list cannot be null or empty.");
            }

            Denominations = denominations
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();
        }
    }
}
