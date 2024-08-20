namespace TestDbMock.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public double TotalAmount { get; set; }
        public string PaymentMethod { get; set; }

        //1-many with User
        public Guid UserOrderId { get; set; }
        public Users User { get; set; }

        //many-many

        public ICollection<ProductOrder> ProductOrders { get; set; }

    }
}
