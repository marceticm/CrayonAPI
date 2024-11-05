using CrayonAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CrayonAPI.UnitTests.Data
{
    public class RepositoryTestBase
    {
        private CrayonDbContext? _context;

        protected async Task<CrayonDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<CrayonDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CrayonDbContext(options);
            await _context.Database.EnsureCreatedAsync();
            return _context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
