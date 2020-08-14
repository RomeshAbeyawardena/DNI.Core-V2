using DNI.Core.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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


    }
}
