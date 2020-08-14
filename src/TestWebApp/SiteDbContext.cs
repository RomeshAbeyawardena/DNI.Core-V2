using DNI.Core.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApp
{
    public class SiteDbContext : EnhancedDbContextBase
    {
        public SiteDbContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {

        }

        public DbSet<User> User { get; set; }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
    }
}
