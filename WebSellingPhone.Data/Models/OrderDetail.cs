namespace WebSellingPhone.Data.Models
{
    public class OrderDetail
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Product Products { get; set; }

        public Guid OrderId { get; set; }   
        public Order Order { get; set; }
    }
}
