﻿using DNI.Core.Contracts.Factories;
using DNI.Core.Services.Abstractions;
using Microsoft.Extensions.Logging;
using System;

namespace DNI.Core.Services.Implementations.Factories
{
    public class LoggerCacheFactory : ImplementationFactoryBase<Type, ILogger>, ILoggerCacheFactory
    {
        public LoggerCacheFactory(ILoggerFactory loggerFactory)
            : base(null)
        {
            this.loggerFactory = loggerFactory;
        }

        public ILogger<TCategory> GetOrCreateLogger<TCategory>()
        {
            var categoryType = typeof(TCategory);

            if(Dictionary.TryGetValue(categoryType, out var logger))
            {
                return (ILogger<TCategory>) logger;
            }

            if(Dictionary.TryAdd(categoryType, loggerFactory.CreateLogger<TCategory>()))
            {
                return GetOrCreateLogger<TCategory>();
            }

            return null;
        }

        private readonly ILoggerFactory loggerFactory;
    }
}