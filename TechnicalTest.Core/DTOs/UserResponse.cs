namespace TechnicalTest.Core.DTOs
{
    public class UserResponseBasic
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }

    public class UserResponse : UserResponseBasic
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
    }
}
