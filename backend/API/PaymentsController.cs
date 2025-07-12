using Microsoft.AspNetCore.Mvc;
using backend.Application;
using backend.Domain;

namespace backend.API;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IDrinkService _drinkService;
    private readonly IChangeService _changeService;

    public PaymentsController(IDrinkService drinkService, IChangeService changeService)
    {
        _drinkService = drinkService;
        _changeService = changeService;
    }

    [HttpPost("pay")]
    public IActionResult Pay([FromBody] PaymentRequest request)
    {
        if (request.AmountPaid < request.TotalToPay)
            return BadRequest("Monto insuficiente.");

        var changeResult = _changeService.CalculateChange(request.AmountPaid, request.TotalToPay);

        if (!string.IsNullOrEmpty(changeResult.Message) && changeResult.Message.StartsWith("Fallo"))
            return BadRequest(changeResult.Message);

        foreach (var item in request.Items)
        {
            var success = _drinkService.PurchaseDrink(item.Name, item.Quantity);
            if (!success)
                return BadRequest($"No se pudo comprar {item.Name}.");
        }

        return Ok(changeResult);
    }
}
