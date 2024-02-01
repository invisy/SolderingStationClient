using SolderingStation.Entities;

namespace SolderingStation.DAL.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity<uint>;
    IGenericRepositoryWithTypedKey<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>;

    void SaveChanges();
}