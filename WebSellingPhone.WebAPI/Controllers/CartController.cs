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
        public async Task<IActionResult> AddToCart(Guid productId, int quantity)
        {
            try
            {
                await _cartService.AddToCart(productId, quantity);
                return Ok("Product added to cart successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        [HttpPost("remove-from-cart")]
        public IActionResult RemoveFromCart(Guid productId)
        {
            _cartService.RemoveFromCart(productId);
            return Ok("Product removed from cart successfully!");
        }

        [HttpGet("get-cart")]
        public IActionResult GetCart()
        {
            var cart = _cartService.GetCurrentCart();
            return Ok(cart);
        }
    }
}
