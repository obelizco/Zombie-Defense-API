using ZombieDefense.Domain.Primitives;

namespace ZombieDefense.Domain.Entities
{
    public class Eliminated : Entity<Guid>
    {
        public Guid ZombieId { get; set; }
        public Guid SimulationId { get; set; }
        public int PointsEarned { get; set; }
        public DateTime EliminatedAt { get; set; } = DateTime.UtcNow;
    }
}
