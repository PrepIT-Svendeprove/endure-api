using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal abstract class BaseTypeConfiguration<T> : BaseTypeConfiguration<T, Guid>
    where T : BaseModel;

internal abstract class BaseTypeConfiguration<T, TKey> : IEntityTypeConfiguration<T>
    where T : BaseModel<TKey>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Version)
            .IsRowVersion();
    }
}
