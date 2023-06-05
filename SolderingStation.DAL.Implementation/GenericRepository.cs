using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolderingStation.DAL.Abstractions;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity<uint>
{
    private readonly SolderingStationDbContext _dbContext;
    private readonly ISpecificationEvaluator _specificationEvaluator;
    
    public GenericRepository(SolderingStationDbContext dbContext)
    {
        _dbContext = dbContext;
        _specificationEvaluator = SpecificationEvaluator.Default;
    }
    
    public GenericRepository(SolderingStationDbContext dbContext,
        ISpecificationEvaluator specificationEvaluator)
    {
        _dbContext = dbContext;
        _specificationEvaluator = specificationEvaluator;
    }

    public virtual TEntity Add(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
        return entity;
    }
    
    public virtual async Task<TEntity?> GetByIdAsync(uint id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(new object[] { id });
    }
    
    public async Task<TEntity?> GetBySpecAsync(ISpecification<TEntity> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<TEntity, TResult> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public virtual async Task<IList<TEntity>> GetListAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }
    
    public async Task<IList<TEntity>> GetListBySpecAsync(ISpecification<TEntity> specification)
    {
        var queryResult = await ApplySpecification(specification).ToListAsync();
        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }
    
    public async Task<IList<TResult>> GetListBySpecAsync<TResult>(ISpecification<TEntity, TResult> specification)
    {
        var queryResult = await ApplySpecification(specification).ToListAsync();
        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }
    
    public virtual void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }
    
    public virtual void Delete(TEntity entity)
    {
        var selected = _dbContext.FindAsync<TEntity>(entity.Id).Result;
        if (selected == null)
            throw new ArgumentException(nameof(entity));
        _dbContext.Set<TEntity>().Remove(selected);
    }
    
    public virtual void Delete(uint id)
    {
        var selected = _dbContext.FindAsync<TEntity>(id).Result;
        if (selected == null)
            throw new ArgumentException(nameof(id));
        _dbContext.Set<TEntity>().Remove(selected);
    }
    
    protected virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, bool evaluateCriteriaOnly = false)
    {
        return _specificationEvaluator.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification, evaluateCriteriaOnly);
    }
    
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
    {
        return _specificationEvaluator.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification);
    }
}