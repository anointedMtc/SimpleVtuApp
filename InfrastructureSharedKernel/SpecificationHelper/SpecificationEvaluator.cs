using DomainSharedKernel;
using DomainSharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureSharedKernel.SpecificationHelper;

// this class is in this layer because it has a dependency on EntityFrameworkCore
public class SpecificationEvaluator<TEntity> where TEntity : class  // baseEntity... ideally the constraint should be where TEntity : BaseEntity but because we didn't implement it in ApplicationUser and ApplicationRole, we go for class
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
    {
        var query = inputQuery;

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        // for lambda Include();
        query = specification.Includes.Aggregate(query, (current, include) =>
            current.Include(include));

        // for string relationship entity Include();
        query = specification.IncludeStrings.Aggregate(query, (current, include) =>
            current.Include(include));

        // OrderBy or OrderByDescending... you have to choose one of the two that is why we are using an if...else conditional statement
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        if (specification.GroupBy != null)
        {
            query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
        }

        // PAGINATION
        if (specification.IsPagingEnabled)
        {
            query = query.Skip(specification.Skip.Value)
                         .Take(specification.Take.Value);
        }

        return query;
    }
}