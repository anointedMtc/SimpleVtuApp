using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Infrastructure.Persistence.Config;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.HasKey(b => b.TransferId);


        builder.Property(b => b.WalletDomainEntityId)
            .IsRequired()
            .HasMaxLength(256);


        builder.OwnsOne(p => p.Amount, p =>
        {
            p.Property(pp => pp.Value).IsRequired().HasColumnType("decimal (18,2)");
        });


        builder.Property(b => b.Direction)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);


        builder.Ignore(b => b.DomainEvents);
    }
}
