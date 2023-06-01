using SolderingStation.Entities;

namespace SolderingStation.DAL.Abstractions;

public interface IGenericRepository<TEntity> : IGenericRepositoryWithTypedKey<int, TEntity> where TEntity: BaseEntity<int>
{
}