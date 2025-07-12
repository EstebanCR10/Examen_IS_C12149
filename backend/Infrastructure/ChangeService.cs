using backend.Application;
using backend.Domain;

namespace backend.Infrastructure;

public class ChangeService : IChangeService
{
    private Dictionary<int, int> availableCoins = new()
    {
        { 1000, 0 },
        { 500, 20 },
        { 100, 30 },
        { 50, 50 },
        { 25, 25 }
    };

    public ChangeResult CalculateChange(int amountPaid, int totalToPay)
    {
        int change = amountPaid - totalToPay;
        if (change < 0)
        {
            return new ChangeResult
            {
                TotalChange = 0,
                Message = "Monto insuficiente"
            };
        }

        var result = new Dictionary<int, int>();
        int remaining = change;

        foreach (var denom in availableCoins.Keys.OrderByDescending(d => d).Where(d => d != 1000))
        {
            int needed = remaining / denom;
            int toUse = Math.Min(needed, availableCoins[denom]);
            if (toUse > 0)
            {
                result[denom] = toUse;
                remaining -= toUse * denom;
            }
        }

        if (remaining > 0)
        {
            return new ChangeResult
            {
                TotalChange = change,
                Message = "Fallo al realizar la compra. No hay monedas suficientes para dar vuelto."
            };
        }

        foreach (var pair in result)
        {
            availableCoins[pair.Key] -= pair.Value;
        }

        return new ChangeResult
        {
            TotalChange = change,
            ChangeBreakdown = result,
            Message = "Compra realizada con éxito"
        };
    }
}
