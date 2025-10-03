

namespace ZombieDefense.Domain.Primitives
{
    public interface IEntity<out TId>
    {
        TId Id { get; }
    }
}
