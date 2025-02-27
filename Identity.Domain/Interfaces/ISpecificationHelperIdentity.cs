using SharedKernel.Domain.Interfaces;

namespace Identity.Domain.Interfaces;

public interface ISpecificationHelperIdentity<T> where T : class, IAggregateRoot
{
    Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null);

    Task<T?> FindAsync(ISpecification<T> specification = null);

    Task<int> CountAsync(ISpecification<T> specification);
}
