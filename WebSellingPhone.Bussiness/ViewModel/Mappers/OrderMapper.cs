
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Bussiness.ViewModel.Mappers
{
    public static class OrderMapper
    {
        public static OrderVm ToOrderVm(this Order order)
        {
            return new OrderVm
            {
                Id = order.Id,
                PaymentMethod = order.PaymentMethod,
                TotalAmount = order.TotalAmount,
                UserOrderId = order.UserOrderId,
                // Thêm các thuộc tính khác nếu có
            };
        }

        public static Order ToOrder(this OrderVm orderVm)
        {
            return new Order
            {
                Id = orderVm.Id,
                PaymentMethod = orderVm.PaymentMethod,
                TotalAmount = orderVm.TotalAmount,
                UserOrderId = orderVm.UserOrderId,
                // Thêm các thuộc tính khác nếu có
            };
        }
    }
}
