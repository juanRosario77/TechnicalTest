namespace TechnicalTest.Data.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int Number { get; set; }
        public int CityCode { get; set; }
        public int CountryCode { get; set; }
        public User? User { get; set; }
    }
}
