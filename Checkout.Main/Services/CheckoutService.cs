using Checkout.Main.Services.Implementation;
using Checkout.Models;

namespace Checkout.Main.Services;

public class CheckoutService(IReadOnlyDictionary<string, SaleableItem> _inventory, IReadOnlyDictionary<string, SaleableItemOffer> _offers)
    : ICheckout
{
    private readonly IDictionary<string, LineItem> _basket = new Dictionary<string, LineItem>();

    public void Scan(string item)
    {
        if (_inventory.TryGetValue(item, out var saleableItem))
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
            var numQualifyingItems = lineItem.Quantity / offer.QualifyingThreshold;
            lineItem.CurrentPrice = offer.SpecialPrice * numQualifyingItems;
            lineItem.CurrentPrice += (lineItem.Quantity % offer.QualifyingThreshold)  * saleableItem.UnitPrice;
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