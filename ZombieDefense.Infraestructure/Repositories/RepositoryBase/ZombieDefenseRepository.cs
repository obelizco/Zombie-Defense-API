using Ardalis.Specification.EntityFrameworkCore;
using ZombieDefense.Infraestructure.Persistence.Contexts;

namespace ZombieDefense.Infraestructure.Repositories.RepositoryBase
{
    public class ZombieDefenseRepository<TEntity>(ZombieDefenseContext _defenseContext) : RepositoryBase<TEntity>(_defenseContext), IZombieDefenseRepository<TEntity> where TEntity : class
    {
    }
}
