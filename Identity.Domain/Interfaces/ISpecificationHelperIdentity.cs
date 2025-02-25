using SharedKernel.Domain.Interfaces;

namespace Identity.Domain.Interfaces;

public interface ISpecificationHelperIdentity<T> where T : class, IAggregateRoot
{
    Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null);

    // returns a single T (entity)
    Task<T?> FindAsync(ISpecification<T> specification = null);


    // to help us count the results of pagination
    Task<int> CountAsync(ISpecification<T> specification);
}
