namespace Checkout.Models;

public class SaleableItemOffer
{
    public string SKU { get; set; } 
    public ushort QualifyingThreshold { get; set; }
    public decimal Price { get; set; }
    public DateTime Expiry { get; set; }
}