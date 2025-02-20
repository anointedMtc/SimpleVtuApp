using InfrastructureSharedKernel.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        // we want it to apply those configurations we specified in the Config Folder
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

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }

}
