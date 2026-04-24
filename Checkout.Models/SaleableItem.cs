namespace Checkout.Models;

public class SaleableItem 
{
    public string SKU { get; set; }
    public decimal Price { get; set; }
    public ushort QualifyingOfferThreshold { get; set; }
    public decimal OfferPrice { get; set; }
}