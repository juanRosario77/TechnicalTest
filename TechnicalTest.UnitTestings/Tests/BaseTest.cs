using Microsoft.Extensions.Configuration;
using TechnicalTest.Data;
using TechnicalTest.UnitTestings.Data;

namespace TechnicalTest.UnitTestings.Tests
{
    public class BaseTest
    {
        protected readonly TechnicalTestContext _context;
        protected IConfiguration _configuration;

        public BaseTest()
        {
            _context = new TechnicalTestContextUnit().GetMemoryContext();
        }

        public IConfiguration GetConfiguration()
        {
            if (_configuration == null)
            {
                _configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
            }

            return _configuration;
        }
    }
}