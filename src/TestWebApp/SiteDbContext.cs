using DNI.Core.Services.Abstractions;
using DNI.Core.Shared.Attributes;
using DNI.Core.Shared.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using DNI.Core.Shared.Constants;

namespace TestWebApp
{
    public class SiteDbContext : EnhancedDbContextBase
    {
        public SiteDbContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Encrypt(EncryptionMethod.TwoWay, EncryptionClassification.Personal)]
        public string EmailAddress { get; set; }

        [Encrypt(EncryptionMethod.OneWay, EncryptionClassification.Personal)]
        public string Password { get; set; }

        [Encrypt(EncryptionMethod.TwoWay, EncryptionClassification.Common)]
        public string FirstName { get; set; }

        [Encrypt(EncryptionMethod.TwoWay, EncryptionClassification.Common)]
        public string MiddleName { get; set; }

        [Encrypt(EncryptionMethod.TwoWay, EncryptionClassification.Common)]
        public string LastName { get; set; }

        [GeneratedDefaultValue(Generators.DateTimeOffSetValueGenerator)]
        public DateTimeOffset Created { get; set; }

        [GeneratedDefaultValue(Generators.DateTimeOffSetValueGenerator, setOnUpdate: true)]
        public DateTimeOffset Modified { get; set; }
    }
}
