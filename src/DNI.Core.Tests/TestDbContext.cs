using DNI.Core.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DNI.Core.Tests
{
    public class TestDbContext : EnhancedDbContextBase
    {
        public TestDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}