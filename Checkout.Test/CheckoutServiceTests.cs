using Checkout.Main.Services;
using Checkout.Main.Services.Implementation;
using Checkout.Models;

namespace Checkout.Test;

public class CheckoutServiceTests
{
    private ICheckout _checkout = null!;
    
    private Dictionary<string, SaleableItem> GetInventory()
    {
       return new Dictionary<string, SaleableItem> {
         {"A", new SaleableItem() { SKU = "A", UnitPrice = 50} },
         {"B", new SaleableItem() { SKU = "B", UnitPrice = 30} },
         {"C", new SaleableItem() { SKU = "C", UnitPrice = 20} },
         {"D", new SaleableItem() { SKU = "D", UnitPrice = 15} },
        };
    }
    
    private Dictionary<string, SaleableItemOffer> GetOffers()
    {
       return new Dictionary<string, SaleableItemOffer> {
         {"A", new SaleableItemOffer() { SKU = "A", QualifyingThreshold = 3, SpecialPrice = 130 } },
         {"B", new SaleableItemOffer() { SKU = "B", QualifyingThreshold = 2, SpecialPrice = 45 } }
        };
    }
    
    [Fact]
    public void CheckoutService_CanScanOne()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        _checkout.Scan("A");
        
        Assert.Equal(50m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_CanScanThreeA_NoOffers()
    {
        _checkout = new CheckoutService(GetInventory(), new Dictionary<string,SaleableItemOffer>());
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        
        Assert.Equal(150m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_ScanVariety_NoOffers()
    {
        _checkout = new CheckoutService(GetInventory(), new Dictionary<string, SaleableItemOffer>());
        _checkout.Scan("A");
        _checkout.Scan("B");
        _checkout.Scan("C");
        _checkout.Scan("D");
        
        Assert.Equal(115m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_3AFor130()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        
        Assert.Equal(130m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_5A()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        
        Assert.Equal(230m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_6A()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        
        Assert.Equal(260m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_2BFor45()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        _checkout.Scan("B");
        _checkout.Scan("B");
        
        Assert.Equal(45m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_3B()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        _checkout.Scan("B");
        _checkout.Scan("B");
        _checkout.Scan("B");
        
        Assert.Equal(75m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_2B_3A()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        _checkout.Scan("B");
        _checkout.Scan("B");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        
        Assert.Equal(175m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_3B_3A()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        _checkout.Scan("B");
        _checkout.Scan("B");
        _checkout.Scan("B");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        
        Assert.Equal(205m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_4A_3B_2C_1D()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        
        _checkout.Scan("B");
        _checkout.Scan("B");
        _checkout.Scan("B");
        
        _checkout.Scan("C");
        _checkout.Scan("C");
        
        _checkout.Scan("D");
        
        Assert.Equal(310m, _checkout.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_TryScanningInvalid()
    {
        _checkout = new CheckoutService(GetInventory(), GetOffers());
        Assert.Throws<KeyNotFoundException>(() => _checkout.Scan("AAAA-NOT_EXISTENT"));
    }
}