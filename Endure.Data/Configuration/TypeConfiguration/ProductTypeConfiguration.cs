using Endure.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class ProductTypeConfiguration : BaseTypeConfiguration<Product>
{
    public void Configuration(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);
    }
}
