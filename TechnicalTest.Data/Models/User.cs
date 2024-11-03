namespace TechnicalTest.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime LastLogin { get; set; }
        public string? Token { get; set; }
        public bool IsActive { get; set; }
        public List<Phone>? Phones { get; set; }
    }
}
