namespace CashMasterPOS.BussinesLogic
{
    public static class TenderValidator
    {
        public static void EnsureValid(Dictionary<decimal, int> tender, List<decimal> validDenominations, string? context = null)
        {
            if (tender is null || tender.Count == 0)
            {
                throw new ArgumentException("Tender cannot be null or empty.");
            }

            var invalidDenoms = tender.Keys.Except(validDenominations).ToList();

            if (invalidDenoms.Any())
            {
                var joined = string.Join(", ", invalidDenoms.Select(d => d.ToString("C")));
                var msg = $"Unsupported denominations found: {joined}";

                if (!string.IsNullOrWhiteSpace(context))
                {
                    msg += $" in context: {context}";
                }

                throw new ArgumentException(msg);
            }
        }

        public static bool TryValidate(Dictionary<decimal, int> tender, List<decimal> validDenominations, out string? errorMessage, string? context = null)
        {
            errorMessage = null;

            if (tender is null || tender.Count == 0)
            {
                errorMessage = "Tender cannot be null or empty.";
                return false;
            }

            var invalidDenominations = tender.Keys.Except(validDenominations).ToList();

            if (invalidDenominations.Any())
            {
                var joinedInvalidDenominations = string.Join(", ", invalidDenominations.Select(denomination => denomination.ToString("C")));

                errorMessage = $"Unsupported denominations found: {joinedInvalidDenominations}";

                if (!string.IsNullOrWhiteSpace(context))
                {
                    errorMessage += $" in context: {context}";
                }

                return false;
            }

            return true;
        }
    }
}
