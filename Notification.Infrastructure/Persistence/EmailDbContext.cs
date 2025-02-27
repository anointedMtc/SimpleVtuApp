using MediatR;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;
using SharedKernel.Infrastructure.Persistence.Interceptors;
using System.Reflection;

namespace Notification.Infrastructure.Persistence;

public class EmailDbContext : DbContext
{
    private readonly IMediator _mediator;
    public EmailDbContext(DbContextOptions<EmailDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }


    public DbSet<EmailEntity> EmailEntities { get; set; }

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
