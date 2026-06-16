using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Tekus.Domain.Interfaces;
using Tekus.Infrastructure.Data;

namespace IntegrationTests.Common
{
    /// <summary>
    /// Base class for integration tests that need a real EF Core database. Uses SQLite with an
    /// in-memory connection kept open for the lifetime of the test, so migrations/configurations
    /// behave like they would against SQL Server, without requiring an actual database server.
    /// xUnit creates a new instance of the test class per test, so each test gets a fresh database.
    /// </summary>
    public abstract class SqliteIntegrationTestBase : IDisposable
    {
        private readonly SqliteConnection _connection;

        protected AppDbContext Context { get; }

        protected IEventDispatcher EventDispatcher { get; }

        protected SqliteIntegrationTestBase(IEventDispatcher? eventDispatcher = null)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            EventDispatcher = eventDispatcher ?? new NoOpEventDispatcher();
            Context = new AppDbContext(options, EventDispatcher);
            Context.Database.EnsureCreated();

            // EnsureCreated() also applies the seed data configured via HasData (see SupplierConfiguration).
            // Tests want a clean, fully controlled dataset, so that seed is removed up front.
            Context.Suppliers.RemoveRange(Context.Suppliers);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
            _connection.Dispose();
        }
    }
}
