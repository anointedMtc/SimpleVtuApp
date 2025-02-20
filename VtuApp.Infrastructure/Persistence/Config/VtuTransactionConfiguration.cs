using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Infrastructure.Persistence.Config;

public class VtuTransactionConfiguration : IEntityTypeConfiguration<VtuTransaction>
{
    public void Configure(EntityTypeBuilder<VtuTransaction> builder)
    {
        builder.HasKey(b => b.Id);


        builder.OwnsOne(p => p.Amount, p =>
        {
            p.Property(pp => pp.Value).IsRequired().HasColumnType("decimal (18,2)");
        });

        // ENUM CONVERSION
        //builder.Property(b => b.TypeOfTransaction)
        //    .HasConversion<string>();


        builder.Ignore(b => b.DomainEvents);
    }
}
