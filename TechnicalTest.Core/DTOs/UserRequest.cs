namespace TechnicalTest.Core.DTOs
{
    public class UserRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public List<PhoneRequest>? Phones { get; set; }
    }

    public class UserRequestLogin
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
