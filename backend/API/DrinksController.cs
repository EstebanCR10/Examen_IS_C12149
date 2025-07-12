using Microsoft.AspNetCore.Mvc;
using backend.Application;
using backend.Domain;

namespace backend.API;

[ApiController]
[Route("api/[controller]")]
public class DrinksController : ControllerBase
{
    private readonly IDrinkService _drinkService;

    public DrinksController(IDrinkService drinkService)
    {
        _drinkService = drinkService;
    }

    [HttpGet]
    public ActionResult<List<Drink>> GetDrinks()
    {
        return Ok(_drinkService.GetAvailableDrinks());
    }

    [HttpPost("purchase")]
    public IActionResult Purchase([FromBody] PurchaseRequest request)
    {
        try
        {
            _drinkService.PurchaseDrink(request.Name, request.Quantity);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public class PurchaseRequest
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
