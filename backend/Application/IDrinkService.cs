using backend.Domain;

namespace backend.Application;

public interface IDrinkService
{
    List<Drink> GetAvailableDrinks();
    bool PurchaseDrink(string name, int quantity);

}
