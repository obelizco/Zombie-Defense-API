using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ZombieDefense.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZombieDefense.Infraestructure.Persistence.Contexts
{
    public class ZombieDefenseContext : DbContext
    {
        private readonly AuditLogInterceptor _auditLogInterceptor;
        public ZombieDefenseContext(DbContextOptions<ZombieDefenseContext> options, AuditLogInterceptor auditLogInterceptor)
            : base(options) 
        
        {
            _auditLogInterceptor = auditLogInterceptor;
        }

        public DbSet<Zombie> Zombies { get; set; }
        public DbSet<Simulation> Simulations { get; set; }
        public DbSet<Eliminated> Eliminated { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZombieDefenseContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditLogInterceptor);
        }
    }
}
