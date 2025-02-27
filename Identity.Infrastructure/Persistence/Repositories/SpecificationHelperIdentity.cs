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
        return ApplySpecification(specification);
    }

    public async Task<T?> FindAsync(ISpecification<T> specification = null)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_applicationDbContext.Set<T>().AsQueryable().AsNoTracking(), spec);

    }

    public IQueryable<ApplicationUser> ApplySpecificationAppUser(ISpecification<ApplicationUser> spec)
    {
        return SpecificationEvaluator<ApplicationUser>.GetQuery(_applicationDbContext.Set<ApplicationUser>().AsQueryable().AsNoTracking(), spec);
    }

    public IQueryable<ApplicationRole> ApplySpecificationAppRole(ISpecification<ApplicationRole> spec)
    {
        return SpecificationEvaluator<ApplicationRole>.GetQuery(_applicationDbContext.Set<ApplicationRole>().AsQueryable().AsNoTracking(), spec);
    }
}
