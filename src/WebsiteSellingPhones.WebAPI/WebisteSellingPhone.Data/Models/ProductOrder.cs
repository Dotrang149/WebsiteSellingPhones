namespace TestDbMock.Models
{
    public class ProductOrder
    {
        public Guid ProductId { get; set; }
        public Product Products { get; set; }

        public Guid OrderId { get; set; }   
        public Order Order { get; set; }
    }
}
