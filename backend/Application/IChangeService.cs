using backend.Domain;

namespace backend.Application;

public interface IChangeService
{
    ChangeResult CalculateChange(int amountPaid, int totalToPay);
}
