using System.Linq.Expressions;

namespace SharedKernel.Domain.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    List<Expression<Func<T, object>>>? Includes { get; }
    List<string>? IncludeStrings { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    Expression<Func<T, object>>? GroupBy { get; }

    // FOR PAGINATION
    int? Take { get; }
    int? Skip { get; }
    bool IsPagingEnabled { get; }

}
