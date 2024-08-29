using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.Service;

namespace WebSellingPhone.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add-to-cart")]
        public IActionResult AddToCart(Guid productId, string productName, decimal price, int quantity)
        {
            _cartService.AddToCart(productId, productName, price, quantity);
            return Ok("Product added to cart successfully!");
        }

        /*[HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(Guid productId, int quantity)
        {
            // Lấy thông tin sản phẩm từ ProductService
            var product = await _productService.GetByIdAsync(productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            // Thêm sản phẩm vào giỏ hàng
            _cartService.AddToCart(product.Id, product.Name, product.Price, quantity);

            return Ok("Product added to cart successfully!");
        }*/

        [HttpGet("get-cart")]
        public IActionResult GetCart()
        {
            var cart = _cartService.GetCurrentCart();
            return Ok(cart);
        }
    }
}
