using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public interface ICartService : IBaseService<Cart>
    {
        Task AddToCart(Guid productId, int quantity);
        Cart GetCurrentCart();
        void RemoveFromCart(Guid productId);
    }
}
