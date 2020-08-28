﻿using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Domains
{
    public class RepositoryOptions : IRepositoryOptions
    {
        public bool EnableTracking { get; set; }
        public bool UseDbContextPools { get ; set ; }
        public int PoolSize { get ; set ; }
        public bool SingulariseTableNames { get; set; }
    }
}
