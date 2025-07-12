using backend.Domain;

namespace backend.Application;

public interface IDrinkService
{
    List<Drink> GetAvailableDrinks();
    void PurchaseDrink(string name, int quantity);
}
