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

        public CartService(IUnitOfWork unitOfWork, ILogger<CartService> logger, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger)
        {
            _httpContextAccessor = httpContextAccessor;
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

         public void AddToCart(Guid productId, string productName, decimal price, int quantity)
        {
            var cart = GetCurrentCart();
            cart.AddItem(productId, productName, price, quantity);
            SaveCart(cart);
        }

        private void SaveCart(Cart cart)
        {
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        /*public async Task<PaginatedResult<Cart>> GetByPagingAsync(string filter = "", string sortBy = "", int pageIndex = 1, int pageSize = 10)
        {
            Func<IQueryable<Cart>, IOrderedQueryable<Cart>> orderBy = null;
            switch (sortBy.ToLower())
            {
                case "":
                    
                    break;
                case "date":
                    orderBy = c => c.OrderBy(c => c.CreatedDate);
                    break;
            }

            Expression<Func<Cart, bool>> filterQuery = null;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                filterQuery = c => c.Items.Any(i => i.ProductName.Contains(filter));
            }

            return await GetAsync(filterQuery, orderBy, "", pageIndex, pageSize);
        }*/
    }
}
