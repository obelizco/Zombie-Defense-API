using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZombieDefense.Domain.Entities;

namespace ZombieDefense.Infraestructure.Persistence.Configuration;
public class EliminatedConfiguration : IEntityTypeConfiguration<Eliminated>
{
    public void Configure(EntityTypeBuilder<Eliminated> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Eliminat__3214EC0735C035CA");

        builder.ToTable("Eliminated");

        builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValueSql("(suser_sname())");
        builder.Property(e => e.EliminatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime");
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);

        //builder.HasOne(d => d.Simulation)
        //    .WithMany(d => d.Eliminations)
        //    .HasForeignKey(d => d.SimulationId)
        //    .OnDelete(DeleteBehavior.ClientSetNull)
        //    .HasConstraintName("FK_Eliminated_Simulations");

        //builder.HasOne(d => d.Zombie)
        //    .WithMany(d => d.Eliminations)
        //    .HasForeignKey(d => d.ZombieId)
        //    .OnDelete(DeleteBehavior.ClientSetNull)
        //    .HasConstraintName("FK_Eliminated_Zombie");
    }
}
