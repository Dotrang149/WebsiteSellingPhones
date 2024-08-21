namespace WebSellingPhone.Data.Models
{
    public class Review
    {
        
        public string Comment { get; set; }

        //1-many with User
        public Guid UserId { get; set; }
        public Users User { get; set; }

        public Guid ProductId { get; set; }
        public Product Products { get; set; }



    }
}
