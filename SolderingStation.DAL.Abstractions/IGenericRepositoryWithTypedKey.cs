using SolderingStation.Entities;

namespace SolderingStation.DAL.Abstractions;

public interface IGenericRepositoryWithTypedKey<TKey, TEntity> where TEntity : BaseEntity<TKey>
{
    TEntity GetById(TKey id);
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> FindOne(Specification<TEntity> spec);
    IEnumerable<TEntity> Find(Specification<TEntity> spec);
    void Add(TKey id, TEntity entity);
    void Update(TKey id, TEntity entity);
    void Upsert(TKey id, TEntity entity);
    void Delete(TKey id);
}
