using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tekus.Domain.Entities;

namespace Tekus.Infrastructure.Data.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Services");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.HourlyRate)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(s => s.Supplier)
                .WithMany(s => s.Services)
                .HasForeignKey(s => s.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
