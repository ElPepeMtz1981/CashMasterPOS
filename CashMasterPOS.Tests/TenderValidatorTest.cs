using CashMasterPOS.BussinesLogic;

namespace CashMasterPOS.Tests
{
    public class TenderValidatorTests
    {
        [Fact]
        public void EnsureValid_WithValidTender_DoesNotThrow()
        {
            var tender = new Dictionary<decimal, int> { { 10.00m, 1 }, { 5.00m, 2 } };

            var valid = new List<decimal> { 10.00m, 5.00m, 1.00m };

            TenderValidator.EnsureValid(tender, valid);
        }

        [Fact]
        public void EnsureValid_WithInvalidDenomination_ThrowsArgumentException()
        {
            var tender = new Dictionary<decimal, int> { { 3.00m, 1 } };

            var valid = new List<decimal> { 1.00m, 5.00m };

            var ex = Assert.Throws<System.ArgumentException>(() =>
                TenderValidator.EnsureValid(tender, valid));

            Assert.Contains("Unsupported denominations", ex.Message);
        }

        [Fact]
        public void TryValidate_WithValidTender_ReturnsTrue()
        {
            var tender = new Dictionary<decimal, int> { { 1.00m, 2 } };

            var valid = new List<decimal> { 1.00m, 5.00m };

            var result = TenderValidator.TryValidate(tender, valid, out var error);
            Assert.True(result);
            Assert.Null(error);
        }

        [Fact]
        public void TryValidate_WithInvalidTender_ReturnsFalseAndErrorMessage()
        {
            var tender = new Dictionary<decimal, int> { { 2.00m, 1 } };

            var valid = new List<decimal> { 1.00m };

            var result = TenderValidator.TryValidate(tender, valid, out var error);
            Assert.False(result);
            Assert.NotNull(error);
            Assert.Contains("Unsupported denominations", error);
        }
    }
}