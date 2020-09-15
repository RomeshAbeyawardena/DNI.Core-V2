using DNI.Core.Services.Abstractions;
using DNI.Core.Shared.Attributes;
using DNI.Core.Shared.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;
using DNI.Core.Shared.Constants;
using Dapper.Contrib.Extensions;

namespace TestWebApp
{
    public class SiteDbContext : EnhancedDbContextBase
    {
        public SiteDbContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
    }

    [Table("Log")]
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public int LogLevelId { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string CategoryName { get; set; }
        public string Message { get; set; }

        [GeneratedDefaultValue(Generators.DateTimeOffSetValueGenerator)]
        [Computed]
        public string Created { get; set; }
    }

    [MessagePack.MessagePackObject(true)]
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
