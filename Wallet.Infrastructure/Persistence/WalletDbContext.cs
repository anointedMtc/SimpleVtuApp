using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Persistence.Interceptors;
using System.Reflection;
using Wallet.Domain.Entities;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Infrastructure.Persistence;

public class WalletDbContext : DbContext
{
    private readonly IMediator _mediator;

    public WalletDbContext(DbContextOptions<WalletDbContext> contextOptions, IMediator mediator) 
        : base(contextOptions)
    {
        _mediator = mediator;
    }

    // EntityFramework core 
    public WalletDbContext() { }


    public DbSet<Owner> Owners { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<WalletDomainEntity> WalletDomainEntities { get; set; }


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
