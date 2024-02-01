using System.Linq.Expressions;

namespace SolderingStation.DAL.Abstractions;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> IsSatisfiedBy();
}