using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Interfaces;
using SharedKernel.Infrastructure.SpecificationHelper;

namespace Identity.Infrastructure.Persistence.Repositories;

public sealed class SpecificationHelperIdentity<T> : ISpecificationHelperIdentity<T> where T : class, IAggregateRoot
{
    private readonly IdentityAuthDbContext _applicationDbContext;

    public SpecificationHelperIdentity(IdentityAuthDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }


    public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null)
    {
        // because I know i won't change the entites gotten from this...
        return ApplySpecification(specification);
    }


    public async Task<T?> FindAsync(ISpecification<T> specification = null)
    {
        // we may change the entity gotten from this... so no need to add AsNoTracking... besides, it's just one
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        // because I know i won't change the entites gotten from this...
        return await ApplySpecification(specification).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        // it is possible to add the AsNoTracking here, but then that means it would also affect the FindAsyn() method... which we don't want
        return SpecificationEvaluator<T>.GetQuery(_applicationDbContext.Set<T>().AsQueryable().AsNoTracking(), spec);

    }


    public IQueryable<ApplicationUser> ApplySpecificationAppUser(ISpecification<ApplicationUser> spec)
    {
        // using the DbSet<T> like this would also give you the same thing... however it is like accessing Data from the backstage and you have lost all the extra good stuff that DbContext gives like tracking changes and disposing resources etc...etc...
        //return SpecificationEvaluator<UserCreatedSagaStateInstance>.GetQuery(_dbSetUserCreatedSagaStateInstance.AsQueryable().AsNoTracking(), spec);

        // this is the preferred approcach - using DbContext
        return SpecificationEvaluator<ApplicationUser>.GetQuery(_applicationDbContext.Set<ApplicationUser>().AsQueryable().AsNoTracking(), spec);

    }

    public IQueryable<ApplicationRole> ApplySpecificationAppRole(ISpecification<ApplicationRole> spec)
    {
        // using the DbSet<T> like this would also give you the same thing... however it is like accessing Data from the backstage and you have lost all the extra good stuff that DbContext gives like tracking changes and disposing resources etc...etc...
        //return SpecificationEvaluator<UserCreatedSagaStateInstance>.GetQuery(_dbSetUserCreatedSagaStateInstance.AsQueryable().AsNoTracking(), spec);

        // this is the preferred approcach - using DbContext
        return SpecificationEvaluator<ApplicationRole>.GetQuery(_applicationDbContext.Set<ApplicationRole>().AsQueryable().AsNoTracking(), spec);

    }
}
