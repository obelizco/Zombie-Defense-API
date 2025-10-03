using ZombieDefense.Domain.Entities;

namespace ZombieDefense.Application.Defense.V1.DTOs
{
    public class DefenseResultDto
    {
        public int TotalScore { get; set; }
        public int BulletsUsed { get; set; }
        public int TimeUsed { get; set; }
        public List<Zombie> ZombiesEliminated { get; set; } = new();
    }
}
