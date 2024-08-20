namespace TestDbMock.Models
{
    public class ProductReview
    {
        public Guid ProductId { get; set; }
        public Product Products { get; set; }

        public Guid ReviewId { get; set; }
        public Review Review { get; set; }
    }
}
