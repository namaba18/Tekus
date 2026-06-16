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

            builder.HasData(
                new
                {
                    Id = Guid.Parse("8f1a6b1e-1c2d-4a3e-9f4a-111111111111"),
                    NIT = "900123456-7",
                    Name = "Suministros Andinos S.A.S.",
                    WebPage = "https://suministrosandinos.com",
                    Email = "contacto@suministrosandinos.com"
                },
                new
                {
                    Id = Guid.Parse("8f1a6b1e-1c2d-4a3e-9f4a-222222222222"),
                    NIT = "900654321-0",
                    Name = "Tecnología y Servicios del Caribe Ltda.",
                    WebPage = "https://tecnocaribe.com",
                    Email = "ventas@tecnocaribe.com"
                });
        }
    }
}
