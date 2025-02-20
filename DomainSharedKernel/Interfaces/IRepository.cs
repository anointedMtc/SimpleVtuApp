namespace DomainSharedKernel.Interfaces;

public interface IRepository<T> where T : class, IAggregateRoot
{

    // READ OPERATIONS

    // SPECIFICATION PATTERN

    // returns an IEnumerable List of T (entities)
    Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null);

    // returns a single T (entity)
    Task<T?> FindAsync(ISpecification<T> specification = null);


    // to help us count the results of pagination
    Task<int> CountAsync(ISpecification<T> specification);


   



    // WRITE OPERATIONS
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);


    // For use within handlers
    Task<T?> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetAllAsync();

}
