using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly ILogger<OrderDetailController> _logger;
        public OrderDetailController(IOrderDetailService orderDetailService, ILogger<OrderDetailController> logger)
        {
            _orderDetailService = orderDetailService;
            _logger = logger;
        }


        
        [HttpGet("get-all-order")]
        public async Task<IActionResult> GetQuizzes()
        {
            var orders = await _orderDetailService.GetAllAsync();

            var ordersDetailViewModels = orders.Select(q => new OrderDetailVm()
            {

                Price = q.Price,
                Quantity = q.Quantity,
                ProductId = q.ProductId,
                OrderId = q.OrderId,
            }).ToList();

            return Ok(ordersDetailViewModels);
        }


        
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            try
            {
                var orderDetails = await _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId);
                if (orderDetails == null)
                {
                    return NotFound();
                }

                var orderDetailViewModels = orderDetails.Select(od => new OrderDetailVm
                {
                    Price = od.Price,
                    Quantity = od.Quantity,
                    ProductId = od.ProductId,
                    OrderId = od.OrderId
                }).ToList();

                return Ok(orderDetailViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order details by order ID.");
                return StatusCode(500, "Internal server error.");
            }
        }


        
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetOrderDetailsByProductIdAsync(Guid productId)
        {
            try
            {
                var orderDetails = await _orderDetailService.GetOrderDetailsByProductIdAsync(productId);
                if (orderDetails == null)
                {
                    return NotFound();
                }

                var orderDetailViewModels = orderDetails.Select(od => new OrderDetailVm
                {
                    Price = od.Price,
                    Quantity = od.Quantity,
                    ProductId = od.ProductId,
                    OrderId = od.OrderId
                }).ToList();

                return Ok(orderDetailViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order details by product ID.");
                return StatusCode(500, "Internal server error.");
            }
        }


        
        [HttpPost("create-orderDetail")]
        public async Task<IActionResult> CreateOrderDetailAsync([FromBody] OrderDetailVm orderDetailVm)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (orderDetailVm == null)
                {
                    _logger.LogError("OrderDetailVm is null!");
                    return BadRequest("OrderDetailVm is null!");
                }

                // Tạo OrderDetail từ OrderDetailVm
                var orderDetail = new OrderDetail
                {
                    Price = orderDetailVm.Price,
                    Quantity = orderDetailVm.Quantity,
                    ProductId = orderDetailVm.ProductId,
                    OrderId = orderDetailVm.OrderId
                };

                // Thêm OrderDetail vào cơ sở dữ liệu
                var isSuccess = await _orderDetailService.AddAsync(orderDetail);

                if (isSuccess > 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to create order detail.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order detail.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
