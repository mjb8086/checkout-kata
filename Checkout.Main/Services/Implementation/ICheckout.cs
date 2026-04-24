namespace Checkout.Main.Services.Implementation;

public interface ICheckout
{
    public void Scan(string item);
    public decimal GetTotalPrice();
}