using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.ViewModel.Mappers
{
    public static class OrderDetailMapper
    {
        public static OrderDetailVm ToOrderDetailVm(this OrderDetail orderDetail)
        {
            return new OrderDetailVm
            {
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price,
                // Thêm các thuộc tính khác nếu có
            };
        }

        public static OrderDetail ToOrderDetail(this OrderDetailVm orderDetailVm)
        {
            return new OrderDetail
            {
                OrderId = orderDetailVm.OrderId,
                ProductId = orderDetailVm.ProductId,
                Quantity = orderDetailVm.Quantity,
                Price = orderDetailVm.Price,
                // Thêm các thuộc tính khác nếu có
            };
        }
    }
}