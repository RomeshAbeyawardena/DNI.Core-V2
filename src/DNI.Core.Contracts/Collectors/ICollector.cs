﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Collectors
{
    public interface ICollector<T>
    {
        Expression<Func<Type, bool>> Filter { get; }
        IEnumerable<T> Collect(T type, IEnumerable<T> types);
    }
}
