namespace backend.Domain;

public class PaymentRequest
{
    public int AmountPaid { get; set; }
    public int TotalToPay { get; set; }
    public List<DrinkItem> Items { get; set; } = new();
}

public class DrinkItem
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class ChangeResult
{
    public int TotalChange { get; set; }
    public Dictionary<int, int> ChangeBreakdown { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
