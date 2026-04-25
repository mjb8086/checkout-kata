namespace Checkout.Models;

public class SaleableItemOffer
{
    public string SKU { get; set; } 
    public ushort QualifyingThreshold { get; set; }
    public decimal SpecialPrice { get; set; }
}