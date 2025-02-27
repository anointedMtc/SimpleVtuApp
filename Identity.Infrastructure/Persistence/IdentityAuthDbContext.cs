using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using SharedKernel.Infrastructure.Persistence.Interceptors;

namespace Identity.Infrastructure.Persistence;

public class IdentityAuthDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly IMediator _mediator;
    public IdentityAuthDbContext(DbContextOptions<IdentityAuthDbContext> options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }


    // PUBLISHING EVENTS AFTER SAVING
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // ignore events if no dispatcher provided
        if (_mediator == null) return result;


        await _mediator.DispatchDomainEvents(this);

        return result;
    }

    
}
