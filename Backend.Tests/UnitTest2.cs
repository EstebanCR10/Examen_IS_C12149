using NUnit.Framework;
using backend.Infrastructure;
using backend.Domain;

namespace Tests
{
    public class DrinkServiceTests
    {
        private DrinkService _drinkService;

        [SetUp]
        public void Setup()
        {
            _drinkService = new DrinkService();
        }

        [Test]
        public void PurchaseDrink_Success_WhenStockIsSufficient()
        {
            var result = _drinkService.PurchaseDrink("Coca Cola", 2);

            Assert.IsTrue(result);
        }

        [Test]
        public void PurchaseDrink_Fails_WhenDrinkNotFound()
        {
            var result = _drinkService.PurchaseDrink("Coca que no existe", 1);

            Assert.IsFalse(result);
        }

        [Test]
        public void PurchaseDrink_Fails_WhenStockIsInsufficient()
        {
            var result = _drinkService.PurchaseDrink("Pepsi", 100);

            Assert.IsFalse(result);
        }
    }
}
