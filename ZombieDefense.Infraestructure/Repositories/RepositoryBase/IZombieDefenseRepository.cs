using Ardalis.Specification;

namespace ZombieDefense.Infraestructure.Repositories.RepositoryBase
{
    public interface IZombieDefenseRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
    }
}
