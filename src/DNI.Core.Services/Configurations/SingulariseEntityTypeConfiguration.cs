using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNI.Core.Services.Configurations
{
    public class SingulariseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity: class
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(typeof(TEntity).Name.Singularize());
        }
    }
}
