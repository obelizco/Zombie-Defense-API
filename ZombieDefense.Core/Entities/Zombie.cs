using ZombieDefense.Domain.Primitives;

namespace ZombieDefense.Domain.Entities
{
    public class Zombie: Entity<Guid>
    {
        public string ZombieType { get; set; } = "";
        public int ShotTimeSeconds { get; set; }
        public int BulletsRequired { get; set; }
        public int Score { get; set; }
        public int ThreatLevel { get; set; }
    }
}
