using backend.Application;
using backend.Domain;

namespace backend.Infrastructure;

public class DrinkService : IDrinkService
{
    private readonly List<Drink> _drinks = new()
    {
        new Drink { Name = "Coca Cola", Quantity = 10, Price = 800 },
        new Drink { Name = "Pepsi", Quantity = 8, Price = 750 },
        new Drink { Name = "Fanta", Quantity = 10, Price = 950 },
        new Drink { Name = "Sprite", Quantity = 15, Price = 975 },
    };

    public List<Drink> GetAvailableDrinks() => _drinks;

    public bool PurchaseDrink(string name, int quantity)
    {
        var drink = _drinks.FirstOrDefault(d => d.Name == name);
        if (drink == null || drink.Quantity < quantity)
            return false;

        drink.Quantity -= quantity;
        return true;
    }

}
