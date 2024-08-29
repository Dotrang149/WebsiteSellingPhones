using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public interface ICartService : IBaseService<Cart>
    {
        void AddToCart(Guid productId, string productName, decimal price, int quantity);
        Cart GetCurrentCart();
    }
}
