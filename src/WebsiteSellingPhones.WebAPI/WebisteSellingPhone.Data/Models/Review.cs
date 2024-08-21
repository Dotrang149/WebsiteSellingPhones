namespace TestDbMock.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }

        //1-many with User
        public Guid UserReviewId { get; set; }
        public Users User { get; set; }
        //many-many
        public ICollection<ProductReview> ProductReviews { get; set; }

    }
}
