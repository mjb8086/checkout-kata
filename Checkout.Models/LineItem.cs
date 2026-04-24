namespace Checkout.Models;

public class LineItem
{
    public string SKU { get; set; }
    public int Quantity { get; set; }
    public decimal CurrentPrice { get; set; }
}