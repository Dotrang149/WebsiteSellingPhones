namespace TestDbMock.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
       
        public int Quantity { get; set; }

        //1-1 with User
        public Users Users { get; set; }
        public Guid UserCartId { get; set; }

        //many-many
        public ICollection<ProductCart> ProductCarts { get; set; }
    }
}
