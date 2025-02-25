using SharedKernel.Domain.Interfaces;
using System.Linq.Expressions;

namespace SharedKernel.Domain.HelperClasses;

// this class is here because it is free from depencies... using only Linq and our different specifications that we would be using for searches would be implementing this base class... because we would be using them in the Application Layer, we need it here or higher...(domain layer)
public abstract class BaseSpecification<T> : ISpecification<T>
{
    // just create a base constructor and assign the criteria you wish to evaluate
    // that satisfies the Where clause... 
    public Expression<Func<T, bool>>? Criteria { get; }
    protected BaseSpecification(Expression<Func<T, bool>>? criteria)
    {
        Criteria = criteria;
    }

    // we require an empty base constructor so we can use this without necessarily 
    // providing a criteria
    protected BaseSpecification()
    {

    }

    // PROPERTIES + the Criteria own above

    public List<Expression<Func<T, object>>>? Includes { get; } = new List<Expression<Func<T, object>>>();
    public List<string>? IncludeStrings { get; } = new List<string>();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public Expression<Func<T, object>>? GroupBy { get; private set; }


    // FOR PAGINATION AS WELL
    public int? Take { get; private set; }
    public int? Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; } = false;



    // METHODS

    // lambda include
    protected virtual void AddInclude(Expression<Func<T, object>>? includeExpression)
    {
        Includes.Add(includeExpression);
    }

    // for the string include
    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    // for orderBy
    protected virtual void ApplyOrderBy(Expression<Func<T, object>>? orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    // for OrderByDescending
    protected virtual void ApplyOrderByDescending(Expression<Func<T, object>>? orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    // for GroupBy
    protected virtual void ApplyGroupBy(Expression<Func<T, object>>? groupByExpression)
    {
        GroupBy = groupByExpression;
    }



    // for the pagination
    protected virtual void ApplyPaging(int? skip, int? take)
    {
        Skip = (skip - 1) * take;
        Take = take;
        IsPagingEnabled = true;
    }
}