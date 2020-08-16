using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using DNI.Core.Shared.Attributes;
using DNI.Core.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Providers
{
    public class ModelEncryptionProvider : IModelEncryptionProvider
    {
        public ModelEncryptionProvider(
            IEncryptionService encryptionService, 
            IMapperProvider mapperProvider,
            IEncryptionProfileManager encryptionProfileManager)
        {
            this.encryptionService = encryptionService;
            this.mapperProvider = mapperProvider;
            this.encryptionProfileManager = encryptionProfileManager;
        }

        public TDestination Decrypt<T, TDestination>(T model)
        {
            var mappedModel = mapperProvider.Map<TDestination>(model);

            var destinationType = typeof(TDestination);

            foreach (var propertyTuple in GetEncryptedProperties<T>())
            {
                var property = propertyTuple.Item1; 
                var currentValue = property.GetValue(model);
                if(currentValue != null)
                { 
                    var destinationProperty =  destinationType.GetProperty(property.Name);
                    if(destinationProperty == null)
                        throw new NullReferenceException();

                    destinationProperty.SetValue(
                        mappedModel, 
                        encryptionService.Decrypt(
                            currentValue.ToString(), 
                            GetEncryptionProfile(propertyTuple.Item2.EncryptionClassification)));
                }
            }

            return mappedModel;
        }

        public TDestination Encrypt<T, TDestination>(T model)
        {
            var mappedModel = mapperProvider.Map<TDestination>(model);

            var destinationType = typeof(TDestination);

            foreach (var propertyTuple in GetEncryptedProperties<T>())
            {
                var property = propertyTuple.Item1; 
                var encryptAttribute = propertyTuple.Item2;
                var currentValue = property.GetValue(model);
                if(currentValue != null)
                {
                    var destinationProperty = destinationType.GetProperty(property.Name);
                    var encryptionProfile = GetEncryptionProfile(encryptAttribute.EncryptionClassification);
                    var encryptedValue = encryptAttribute.EncryptionMethod == EncryptionMethod.TwoWay 
                        ? encryptionService.Encrypt(
                            currentValue.ToString(), 
                            encryptionProfile)
                        : encryptionService.Hash(currentValue.ToString(), encryptionProfile);

                    if(destinationProperty == null)
                        throw new NullReferenceException();
                    

                    destinationProperty.SetValue(mappedModel, encryptedValue);
                }
            }

            return mappedModel;
        }

        private IEnumerable<Tuple<PropertyInfo, EncryptAttribute>> GetEncryptedProperties<T>()
        {
            var encryptAttributeType = typeof(EncryptAttribute);
            var modelType = typeof(T);

            var properties = modelType.GetProperties().Where(property => property.GetCustomAttributes().Any(attribute => attribute.GetType() == encryptAttributeType));

            return properties.Select(property => Tuple.Create(property, property.GetCustomAttribute<EncryptAttribute>()));
        }

        private IEncryptionProfile GetEncryptionProfile(EncryptionClassification encryptionClassification)
        {
            if(encryptionProfileManager.TryGetValue(encryptionClassification, out var encryptionProfile))
            {
                return encryptionProfile;
            }

            throw new NotSupportedException();
        }

        private readonly IEncryptionService encryptionService;
        private readonly IMapperProvider mapperProvider;
        private readonly IEncryptionProfileManager encryptionProfileManager;
    }
}
