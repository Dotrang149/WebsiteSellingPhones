namespace WebSellingPhone.Data.Models
{
    public class Promotion
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
