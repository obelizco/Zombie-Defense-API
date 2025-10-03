using ZombieDefense.Domain.Primitives;

namespace ZombieDefense.Domain.Entities
{
    public class Simulation: Entity<Guid>
    {
        public DateTime SimulationDate { get; set; } = DateTime.UtcNow;
        public int TimeAvailableSeconds { get; set; }
        public int BulletsAvailable { get; set; }
    }
}
