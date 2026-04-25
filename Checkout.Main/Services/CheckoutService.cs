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
            throw new Exception("Inventory is empty");
        }

        if (_inventory.TryGetValue(item, out SaleableItem saleableItem))
        {
            AddItemToBasket(saleableItem);
        }
        else
        {
            throw new Exception("Item not found in inventory.");
        }
    }

    void AddItemToBasket(SaleableItem saleableItem)
    {
        if (_basket.ContainsKey(saleableItem.SKU))
        {
            // todo - offer logic
            var itemRef = _basket[saleableItem.SKU];
            itemRef.Quantity++;
            itemRef.CurrentPrice = saleableItem.UnitPrice * itemRef.Quantity;
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

    public decimal GetTotalPrice()
    {
        return _basket.Values.Sum(x => x.CurrentPrice);
    }
}