namespace TestDbMock.Models
{
    public class Product
    {
        public Guid Id { get; set; }
      
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image {  get; set; }

        //1-many with brand
        public Guid BrandProductId { get; set; }
        public Brand Brand { get; set; }

        //1-many with promotion
        public Guid PromotionProductId { get; set; }
        public Promotion Promotion { get; set; }

        //many-many
        public ICollection<ProductOrder> ProductOrders { get; set; }   
        public ICollection<ProductReview> ProductReviews { get; set; }

        public ICollection<ProductCart> ProductCarts { get; set; }
    }
}
