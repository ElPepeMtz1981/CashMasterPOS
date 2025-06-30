using CashMasterPOS.BussinesLogic;
using CashMasterPOS.Exceptions;

namespace CashMaster.Tests
{
    public class ChangeCalculatorTests
    {
        public ChangeCalculatorTests()
        {
            // Configurar denominaciones antes de cada prueba
            Currency.Denominations = new List<decimal>
            {
                100.00m, 50.00m, 20.00m, 10.00m, 5.00m,
                 1.00m, 0.50m, 0.25m, 0.10m, 0.05m, 0.01m
            };
        }

        [Fact]
        public void CalculateChange_ExactPayment_ReturnsEmpty()
        {
            var tender = new Dictionary<decimal, int> { { 10.00m, 1 } };
            var result = ChangeCalculator.CalculateChange(10.00m, tender);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CalculateChange_ValidChange_ReturnsCorrectBreakdown()
        {
            var tender = new Dictionary<decimal, int> { { 20.00m, 1 } };
            var result = ChangeCalculator.CalculateChange(13.75m, tender);

            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[5.00m]);
            Assert.Equal(1, result[1.00m]);
            Assert.Equal(1, result[0.25m]);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void CalculateChange_InvalidPrice_ThrowsException(decimal price)
        {
            var tender = new Dictionary<decimal, int> { { 10.00m, 1 } };

            var ex = Assert.Throws<ChangeCalculationException>(() =>
                ChangeCalculator.CalculateChange(price, tender));

            Assert.Equal(price, ex.Price);
            Assert.Equal(0, ex.Payment);
        }

        [Fact]
        public void CalculateChange_NullTender_ThrowsException()
        {
            var ex = Assert.Throws<ChangeCalculationException>(() =>
                ChangeCalculator.CalculateChange(10.00m, null));

            Assert.Equal(10.00m, ex.Price);
            Assert.Equal(0, ex.Payment);
        }

        [Fact]
        public void CalculateChange_InsufficientPayment_ThrowsException()
        {
            var tender = new Dictionary<decimal, int> { { 1.00m, 2 } };

            var ex = Assert.Throws<ChangeCalculationException>(() =>
                ChangeCalculator.CalculateChange(5.00m, tender));

            Assert.Equal(5.00m, ex.Price);
            Assert.Equal(2.00m, ex.Payment);
            Assert.Contains("Insufficient", ex.Message);
        }

        [Fact]
        public void CalculateChange_CannotMakeExactChange_ThrowsException()
        {
            // Eliminar denominaciones pequeñas
            Currency.Denominations = new List<decimal> { 1.00m };

            var tender = new Dictionary<decimal, int> { { 5.00m, 1 } };

            var ex = Assert.Throws<ChangeCalculationException>(() =>
                ChangeCalculator.CalculateChange(3.50m, tender));

            Assert.Equal(5.00m, ex.Payment);
            Assert.Equal(3.50m, ex.Price);
            Assert.Contains("Exact change cannot be returned", ex.Message);
        }
    }
}