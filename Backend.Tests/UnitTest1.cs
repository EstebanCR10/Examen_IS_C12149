using NUnit.Framework;
using backend.Infrastructure;
using backend.Domain;

namespace Tests
{
    public class ChangeServiceTests
    {
        private ChangeService _changeService;

        [SetUp]
        public void Setup()
        {
            _changeService = new ChangeService();
        }

        [Test]
        public void CalculateChange_ReturnsCorrectChange()
        {
            var result = _changeService.CalculateChange(2000, 1350);

            Assert.AreEqual(650, result.TotalChange);
            Assert.AreEqual("Compra realizada con éxito", result.Message);
            Assert.IsTrue(result.ChangeBreakdown.ContainsKey(500));
        }

        [Test]
        public void CalculateChange_Fails_WhenInsufficientCoins()
        {
            var result = _changeService.CalculateChange(2001, 1000);

            Assert.AreEqual(1001, result.TotalChange);
            Assert.AreEqual("Fallo al realizar la compra. No hay monedas suficientes para dar vuelto.", result.Message);
        }

        [Test]
        public void CalculateChange_Fails_WhenAmountIsTooLow()
        {
            var result = _changeService.CalculateChange(500, 1000);

            Assert.AreEqual(0, result.TotalChange);
            Assert.AreEqual("Monto insuficiente", result.Message);
        }
    }
}
