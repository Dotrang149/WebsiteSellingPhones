namespace TestDbMock.Models
{
    public class ProductCart
    {
        public Guid ProductId { get; set; }
        public Product Products { get; set; }

        public Guid CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
