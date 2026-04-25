using Checkout.Main.Services.Implementation;
using Checkout.Models;

namespace Checkout.Main.Services;

public class CheckoutService: ICheckout
{
    private IReadOnlyDictionary<string, SaleableItem> _inventory;
    private IReadOnlyDictionary<string, SaleableItemOffer> _offers;
    private IDictionary<string, LineItem> _basket;
    
    public CheckoutService(IReadOnlyDictionary<string, SaleableItem> inventory, IReadOnlyDictionary<string, SaleableItemOffer> offers)
    {
        _inventory = inventory;
        _offers = offers;
        _basket = new Dictionary<string, LineItem>();
    }
    
    public void Scan(string item)
    {
        if (_inventory.Count < 1)
        {
            throw new InvalidOperationException("Inventory is empty");
        }

        if (_inventory.TryGetValue(item, out SaleableItem saleableItem))
        {
            AddItemToBasket(saleableItem);
        }
        else
        {
            throw new KeyNotFoundException("Item not found in inventory.");
        }
    }

    void AddItemToBasket(SaleableItem saleableItem)
    {
        if (_basket.TryGetValue(saleableItem.SKU, out var lineItem))
        {
            lineItem.Quantity++;
            ApplyOffers(lineItem, saleableItem);
        }
        else
        {
            _basket[saleableItem.SKU] = new()
            {
                SKU = saleableItem.SKU,
                Quantity = 1,
                CurrentPrice = saleableItem.UnitPrice
            };
        }
    }

    void ApplyOffers(LineItem lineItem, SaleableItem saleableItem)
    {
        if (_offers.TryGetValue(lineItem.SKU, out var offer))
        {
            var numOffers = lineItem.Quantity / offer.QualifyingThreshold;
            var numLoose = lineItem.Quantity % offer.QualifyingThreshold;
            lineItem.CurrentPrice = offer.SpecialPrice * numOffers;
            lineItem.CurrentPrice += numLoose * saleableItem.UnitPrice;
        }
        else
        {
            lineItem.CurrentPrice = saleableItem.UnitPrice * lineItem.Quantity;
        }
    }

    public decimal GetTotalPrice()
    {
        return _basket.Values.Sum(x => x.CurrentPrice);
    }
}