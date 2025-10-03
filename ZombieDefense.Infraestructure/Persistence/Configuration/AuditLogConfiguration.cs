using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Domain.Primitives;

namespace ZombieDefense.Infraestructure.Persistence.Configuration;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC0754522003");

        builder.Property(e => e.ActionDate)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
        builder.Property(e => e.ActionType).HasMaxLength(10);
        builder.Property(e => e.CreateAt)
            .HasMaxLength(100)
            .HasDefaultValueSql("(suser_sname())");
    }
}
