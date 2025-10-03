using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Domain.Primitives;

namespace ZombieDefense.Infraestructure.Persistence.Configuration;
public class SimulationConfiguration : IEntityTypeConfiguration<Simulation>
{
    public void Configure(EntityTypeBuilder<Simulation> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Simulati__3214EC071361B8ED");

        builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValueSql("(suser_sname())");
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.SimulationDate)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
        builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);
    }
}