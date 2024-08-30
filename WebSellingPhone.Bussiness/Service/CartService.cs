using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebSellingPhone.Bussiness.Extensions;
using WebSellingPhone.Bussiness.Service.Base;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Infrastructure;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.Service
{
    public class CartService : BaseService<Cart>, ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;

        public CartService(IUnitOfWork unitOfWork, ILogger<CartService> logger, IHttpContextAccessor httpContextAccessor, IProductService productService)
            : base(unitOfWork, logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }

        public Cart GetCurrentCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cart = session.GetObjectFromJson<Cart>("Cart");

            if (cart == null)
            {
                cart = new Cart();
                session.SetObjectAsJson("Cart", cart);
            }

            return cart;
        }

        public async Task AddToCart(Guid productId, int quantity)
        {
            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var cart = GetCurrentCart();
            cart.AddItem(productId, product.Name, product.Price, quantity);
            SaveCart(cart);
        }

        public void RemoveFromCart(Guid productId)
        {
            var cart = GetCurrentCart();
            cart.RemoveItem(productId);
            SaveCart(cart);
        }

        private void SaveCart(Cart cart)
        {
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart);
        }
    }

}