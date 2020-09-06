using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Shared.Attributes;
using DNI.Core.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.CompilerServices;
using Humanizer;

[assembly: InternalsVisibleTo("DNI.Core.Tests", AllInternalsVisible = true)]
namespace DNI.Core.Services.Abstractions
{
    public abstract class EnhancedDbContextBase : DbContext
    {
        protected EnhancedDbContextBase(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
            entityEntrySubject = new Subject<EntityEntry>();
            entityEntrySubject.Subscribe(OnNextEntity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var repositoryOptions = this.GetService<IRepositoryOptions>();

            if (repositoryOptions.SingulariseTableNames)
            {
                 foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                 {
                      entityType.SetTableName(entityType.GetTableName().Singularize());
                 }
            }

            base.OnModelCreating(modelBuilder);
        }

        public void ReportChange<TEntity>(EntityEntry<TEntity> entry)
            where TEntity : class
        {
            entityEntrySubject.OnNext(entry);
        }

        private void OnNextEntity(EntityEntry entry)
        {
            PrepareForAddOrUpdate(entry.Entity, entry.State);
        }

        internal void PrepareForAddOrUpdate<TEntity>(TEntity entity, EntityState state)
        {
            var entityType = typeof(TEntity);
            
            if (entityType == typeof(object))
            {
                entityType = entity.GetType();
            }

            var implementedDecoratedValueGenerators = GetImplementedDecoratedValueGenerators(entityType.GetProperties());
            SetImplementedDecoratedValueUsingSpecifiedGenerators(entity, state, implementedDecoratedValueGenerators);
        }

        private void SetImplementedDecoratedValueUsingSpecifiedGenerators<TEntity>(
            TEntity entity, 
            EntityState entityState,
            IDictionary<PropertyInfo, Tuple<GeneratedDefaultValueAttribute, IValueGenerator>> generatorDictionary)
        {
            foreach(var (property, valueGenerator) in generatorDictionary)
            {
                if(valueGenerator != null) 
                {
                    var currentValue = property.GetValue(entity);
                    if(valueGenerator.Item2.ExpectsValue 
                        || (valueGenerator.Item1.SetOnUpdate && entityState == EntityState.Modified) 
                        || currentValue.IsDefault())
                    { 
                        property.SetValue(entity, valueGenerator.Item2.GenerateValue(currentValue));
                    }
                }
            }
        }

        private IDictionary<PropertyInfo, Tuple<GeneratedDefaultValueAttribute,IValueGenerator>> GetImplementedDecoratedValueGenerators(
            IEnumerable<PropertyInfo> propertiesToScan)
        {
            var valueGeneratorProvider = this.GetService<IValueGeneratorProvider>();
            var valueGeneratorDictionary = new Dictionary<PropertyInfo, Tuple<GeneratedDefaultValueAttribute, IValueGenerator>>();
            var generatedDefaultValueAttributes = propertiesToScan.ToDictionary(property => property, property => property.GetCustomAttribute<GeneratedDefaultValueAttribute>());

            foreach(var generatedDefaultValueAttribute in generatedDefaultValueAttributes)
            {
                if(generatedDefaultValueAttribute.Value != null)
                { 
                    var valueGenerator = valueGeneratorProvider.GetValueGeneratorByName(generatedDefaultValueAttribute.Value.GeneratorName, 
                        generatedDefaultValueAttribute.Value.UsesDefaultServiceInjector);
                    var tuple = Tuple.Create(generatedDefaultValueAttribute.Value, valueGenerator);
                    valueGeneratorDictionary.Add(generatedDefaultValueAttribute.Key, tuple);
                }
            }

            return valueGeneratorDictionary;
        }

        private ISubject<EntityEntry> entityEntrySubject;
    }
}
