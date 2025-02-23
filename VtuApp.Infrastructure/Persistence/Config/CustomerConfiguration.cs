using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Infrastructure.Persistence.Config;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(b => b.CustomerId);

        builder.Metadata.FindNavigation(nameof(Customer.VtuTransactions))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(p => p.TotalBalance, p =>
        {
            p.Property(pp => pp.Value).IsRequired().HasColumnType("decimal (18,2)");
        });

        builder.OwnsOne(p => p.BonusBalance, p =>
        {
            p.Property(pp => pp.Value).IsRequired().HasColumnType("decimal (18,2)");
        });

        // NON-CLUSTERED
        //builder.HasIndex(b => b.Email)
        //     .IsUnique();

        builder.Ignore(b => b.DomainEvents);
    }
}
