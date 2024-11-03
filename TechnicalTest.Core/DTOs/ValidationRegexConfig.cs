namespace TechnicalTest.Core.DTOs
{
    public class ValidationConfig
    {
        public string? EmailRegex { get; set; }
        public string? PasswordRegex { get; set; }
        public short PasswordMinLengh { get; set; }
        public short PasswordMaxLengh { get; set; }
    }
}