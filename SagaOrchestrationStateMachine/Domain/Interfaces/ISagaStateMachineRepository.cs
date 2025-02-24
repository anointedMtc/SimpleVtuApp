﻿using DomainSharedKernel.Interfaces;

namespace SagaOrchestrationStateMachines.Domain.Interfaces;

public interface ISagaStateMachineRepository<T> where T : class
{

    Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null);

    Task<T?> FindAsync(ISpecification<T> specification = null);

    Task<int> CountAsync(ISpecification<T> specification);



    // WRITE OPERATIONS
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);


    // For use within handlers
    Task<T?> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetAllAsync();

}
