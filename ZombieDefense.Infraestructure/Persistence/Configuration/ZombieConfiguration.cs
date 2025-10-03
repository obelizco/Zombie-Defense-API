using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Domain.Primitives;

namespace ZombieDefense.Infraestructure.Persistence.Configuration;

public class ZombieConfiguration : IEntityTypeConfiguration<Zombie>
{
    public void Configure(EntityTypeBuilder<Zombie> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Zombies__3214EC071BCA0CAB");

        builder.Property(e => e.Id).HasDefaultValueSql("(newid())");

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValueSql("(suser_sname())");
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);
        builder.Property(e => e.ZombieType).HasMaxLength(50);
    }
}
    

