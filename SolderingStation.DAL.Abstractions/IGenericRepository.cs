using SolderingStation.Entities;

namespace SolderingStation.DAL.Abstractions;

public interface IGenericRepository<TEntity> : IGenericRepositoryWithTypedKey<uint, TEntity> where TEntity: BaseEntity<uint>
{
}