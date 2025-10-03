using ZombieDefense.Domain.Primitives;

namespace ZombieDefense.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public Guid EliminatedId { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string CreateAt { get; set; } = string.Empty;
        public string OldValues { get; set; } = string.Empty;
        public string NewValues { get; set; } = string.Empty;
        public Eliminated? Eliminated { get; set; }
    }
}
