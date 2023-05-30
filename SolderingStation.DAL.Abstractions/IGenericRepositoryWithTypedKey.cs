using Ardalis.Specification;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Abstractions;

public interface IGenericRepositoryWithTypedKey<TKey, TEntity> where TEntity : BaseEntity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<TEntity?> GetBySpecAsync(ISpecification<TEntity> specification);
    Task<TResult?> GetBySpecAsync<TResult>(ISpecification<TEntity, TResult> specification);
    Task<IList<TEntity>> GetListAsync();
    Task<IList<TEntity>> GetListBySpecAsync(ISpecification<TEntity> specification);
    Task<IList<TResult>> GetListBySpecAsync<TResult>(ISpecification<TEntity, TResult> specification);
    TEntity Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TKey id);
    void Delete(TEntity id);
}