using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DNI.Core.Tests", AllInternalsVisible = true)]
namespace DNI.Core.Services.Abstractions
{
    public abstract class EnhancedDbContextBase : DbContext
    {
        protected EnhancedDbContextBase(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            PrepareForAddOrUpdate(entity);
            return base.Add(entity);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            PrepareForAddOrUpdate(entity);
            return base.Update(entity);
        }
        
        internal void PrepareForAddOrUpdate<TEntity>(TEntity entity)
        {
            var entityType = typeof(TEntity);

            var implementedDecoratedValueGenerators = GetImplementedDecoratedValueGenerators(entityType.GetProperties());
            SetImplementedDecoratedValueUsingSpecifiedGenerators(entity, implementedDecoratedValueGenerators);
        }

        private void SetImplementedDecoratedValueUsingSpecifiedGenerators<TEntity>(TEntity entity, 
            IDictionary<PropertyInfo, IValueGenerator> generatorDictionary)
        {
            foreach(var (property, valueGenerator) in generatorDictionary)
            {
                if(valueGenerator != null) 
                {
                    var currentValue = property.GetValue(entity);
                    property.SetValue(entity, valueGenerator.GenerateValue(currentValue));
                }
            }
        }

        private IDictionary<PropertyInfo, IValueGenerator> GetImplementedDecoratedValueGenerators(IEnumerable<PropertyInfo> propertiesToScan)
        {
            var valueGeneratorProvider = this.GetService<IValueGeneratorProvider>();
            var valueGeneratorDictionary = new Dictionary<PropertyInfo, IValueGenerator>();
            var generatedDefaultValueAttributes = propertiesToScan.ToDictionary(property => property, property => property.GetCustomAttribute<GeneratedDefaultValueAttribute>());

            foreach(var generatedDefaultValueAttribute in generatedDefaultValueAttributes)
            {
                if(generatedDefaultValueAttribute.Value != null)
                { 
                    valueGeneratorDictionary.Add(generatedDefaultValueAttribute.Key, 
                            valueGeneratorProvider.GetValueGeneratorByName(generatedDefaultValueAttribute.Value.GeneratorName, 
                        generatedDefaultValueAttribute.Value.UsesDefaultServiceInjector));
                }
            }

            return valueGeneratorDictionary;
        }
    }
}
