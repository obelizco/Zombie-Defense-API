namespace ZombieDefense.Domain.Primitives
{
    public abstract class Entity<TId> : IEntity<TId>
    {
        public TId Id { get; protected init; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "system";
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
