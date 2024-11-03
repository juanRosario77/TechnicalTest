using Microsoft.EntityFrameworkCore;
using TechnicalTest.Data;

namespace TechnicalTest.UnitTestings.Data
{
    public class TechnicalTestContextUnit
    {
        private readonly TechnicalTestContext _context;

        public TechnicalTestContextUnit()
        {
            var options = new DbContextOptionsBuilder<TechnicalTestContext>()
                .UseInMemoryDatabase(databaseName: "UnitTestDB")
                .Options;

            _context = new TechnicalTestContext(options);
        }

        public TechnicalTestContext GetMemoryContext()
        {
            return _context;
        }
    }
}