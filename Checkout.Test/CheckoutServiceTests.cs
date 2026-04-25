using System.Collections.Frozen;
using Checkout.Main.Services;
using Checkout.Models;

namespace Checkout.Test;

public class CheckoutServiceTests
{
    private FrozenDictionary<string, SaleableItem> GetInventory()
    {
       var inventory = new Dictionary<string, SaleableItem> {
         {"A", new SaleableItem() { SKU = "A", UnitPrice = 50} },
         {"B", new SaleableItem() { SKU = "B", UnitPrice = 30} },
         {"C", new SaleableItem() { SKU = "C", UnitPrice = 20} },
         {"D", new SaleableItem() { SKU = "D", UnitPrice = 15} },
        };
        return inventory.ToFrozenDictionary();
    }
    
    private FrozenDictionary<string, SaleableItemOffer> GetOffers()
    {
       var offerLookup = new Dictionary<string, SaleableItemOffer> {
         {"A", new SaleableItemOffer() { SKU = "A", QualifyingThreshold = 3, SpecialPrice = 130 } },
         {"B", new SaleableItemOffer() { SKU = "B", QualifyingThreshold = 2, SpecialPrice = 45 } }
        };
        return offerLookup.ToFrozenDictionary();
    }
    
    [Fact]
    public void CheckoutService_CanScanOne()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        checkoutService.Scan("A");
        
        Assert.Equal(50m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_CanScanThreeA_NoOffers()
    {
        var checkoutService = new CheckoutService(GetInventory(), new Dictionary<string,SaleableItemOffer>());
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        
        Assert.Equal(150m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_ScanVariety_NoOffers()
    {
        var checkoutService = new CheckoutService(GetInventory(), new Dictionary<string, SaleableItemOffer>());
        checkoutService.Scan("A");
        checkoutService.Scan("B");
        checkoutService.Scan("C");
        checkoutService.Scan("D");
        
        Assert.Equal(115m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_3AFor130()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        
        Assert.Equal(130m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_5A()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        
        Assert.Equal(230m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_6A()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        
        Assert.Equal(260m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_2BFor45()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        checkoutService.Scan("B");
        checkoutService.Scan("B");
        
        Assert.Equal(45m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_3B()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        checkoutService.Scan("B");
        checkoutService.Scan("B");
        checkoutService.Scan("B");
        
        Assert.Equal(75m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_2B_3A()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        checkoutService.Scan("B");
        checkoutService.Scan("B");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        
        Assert.Equal(175m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_3B_3A()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        checkoutService.Scan("B");
        checkoutService.Scan("B");
        checkoutService.Scan("B");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        
        Assert.Equal(205m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_TryScanningInvalid()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        Assert.Throws<KeyNotFoundException>(() => checkoutService.Scan("AAAA-NOT_EXISTENT"));
    }
}