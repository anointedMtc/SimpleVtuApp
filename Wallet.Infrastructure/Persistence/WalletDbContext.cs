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
    //private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public WalletDbContext(DbContextOptions<WalletDbContext> contextOptions, IMediator mediator) 
        : base(contextOptions)
    {
        _mediator = mediator;
        //_auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    // EntityFramework core 
    public WalletDbContext() { }


    public DbSet<Owner> Owners { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<WalletDomainEntity> WalletDomainEntities { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
       
        // we want it to apply those configurations we specified in the Config Folder
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // I moved the configuration to the config class
        // this relationship is starting from the Pricipal/Parent... but you can choose
        // to use the dependent/child class  and no one is better than the other... just
        // choose one

        //modelBuilder.Entity<Owner>()
        //    .HasOne(e => e.WalletDomainEntity)
        //    .WithOne(e => e.Owner)
        //    .HasForeignKey<WalletDomainEntity>(e => e.OwnerId)
        //    .IsRequired();

        // OR from dependent/child class

        //modelBuilder.Entity<WalletDomainEntity>()
        //    .HasOne(d => d.Owner)
        //    .WithOne(e => e.WalletDomainEntity)
        //    .HasForeignKey<WalletDomainEntity>(d => d.OwnerId)
        //    .IsRequired();

        // Neither of these options is better than the other; they both result in exactly the same configuration.

    }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);

    //    //optionsBuilder.UseSqlServer("Data Source =CHIKURDEE\\SQLEXPRESS;Initial Catalog=vtuApp_WalletApiModuleDb;Integrated Security=True;TrustServerCertificate=True;Trusted_Connection=True;Connection Timeout=30;");
    //}


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
