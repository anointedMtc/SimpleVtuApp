using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Infrastructure.Persistence.Config;

public class WalletDomainEntityConfiguration : IEntityTypeConfiguration<WalletDomainEntity>
{
    public void Configure(EntityTypeBuilder<WalletDomainEntity> builder)
    {

        builder.HasKey(b => b.WalletId);

        builder.Property(b => b.OwnerId)
           .IsRequired()
           .HasMaxLength(256);


        //var navigation = builder.Metadata.FindNavigation(nameof(WalletDomainEntity.Transfers));
        //navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(WalletDomainEntity.Transfers))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        // NON-CLUSTERED
        builder.HasIndex(b => b.Email)
             .IsUnique();

        builder.Ignore(b => b.DomainEvents);
    }
}
