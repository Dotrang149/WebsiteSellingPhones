namespace TestDbMock.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool isActive { get; set; }

        //1-1 with Cart
        public Cart Cart { get; set; }
    

        //1-many
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
