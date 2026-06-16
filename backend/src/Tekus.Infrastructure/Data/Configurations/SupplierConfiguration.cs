using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tekus.Domain.Entities;

namespace Tekus.Infrastructure.Data.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.NIT)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.WebPage)
                .HasMaxLength(250);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(s => s.NIT).IsUnique();
        }
    }
}
