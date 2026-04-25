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
    public void CheckoutService_CanScanThreeA()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        checkoutService.Scan("A");
        
        Assert.Equal(150m, checkoutService.GetTotalPrice());
    }
    
    [Fact]
    public void CheckoutService_TryScanningInvalid()
    {
        var checkoutService = new CheckoutService(GetInventory(), GetOffers());
        Assert.Throws<KeyNotFoundException>(() => checkoutService.Scan("AAAA-NOT_EXISTENT"));
    }
}