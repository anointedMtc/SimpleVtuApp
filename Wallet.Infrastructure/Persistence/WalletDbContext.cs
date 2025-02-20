using InfrastructureSharedKernel.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Wallet.Domain.Entities;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Infrastructure.Persistence;

public class WalletDbContext : DbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public WalletDbContext(DbContextOptions<WalletDbContext> options, 
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) 
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }


    public DbSet<Owner> Owners { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<WalletDomainEntity> WalletDomainEntities { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
       
        // we want it to apply those configurations we specified in the Config Folder
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }


    // PUBLISHING EVENTS BEFORE SAVING
    //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    await _mediator.DispatchDomainEvents(this);

    //    return await base.SaveChangesAsync(cancellationToken);
    //}


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
