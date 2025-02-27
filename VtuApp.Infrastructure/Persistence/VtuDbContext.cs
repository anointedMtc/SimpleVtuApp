using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Persistence.Interceptors;
using System.Reflection;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Infrastructure.Persistence;

public class VtuDbContext : DbContext
{
    private readonly IMediator _mediator;
    public VtuDbContext(DbContextOptions<VtuDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<VtuTransaction> VtuTransactions { get; set; }

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
