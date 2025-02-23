using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Infrastructure.Persistence.Config;

public class WalletDomainEntityConfiguration : IEntityTypeConfiguration<WalletDomainEntity>
{
    public void Configure(EntityTypeBuilder<WalletDomainEntity> builder)
    {

        builder.HasKey(b => b.WalletDomainEntityId);

        builder.Property(b => b.OwnerId)
           .IsRequired()
           .HasMaxLength(256);


        //var navigation = builder.Metadata.FindNavigation(nameof(WalletDomainEntity.Transfers));
        //navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(WalletDomainEntity.Transfers))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        // NON-CLUSTERED
        //builder.HasIndex(b => b.Email)
        //     .IsUnique();


        // JUST CONFIGURE IT ONLY ONCE... I USED THE PRINCIPAL/PARENT CLASS SO NO NEED TO DO IT AGAIN HERE (DEPENDENT/CHILD CLASS)
        // Neither of these options is better than the other; they both result in exactly the same configuration.
        //builder.HasOne(d => d.Owner)
        //    .WithOne(e => e.WalletDomainEntity)
        //    .HasForeignKey<WalletDomainEntity>(e => e.OwnerId)
        //    .IsRequired();


        builder.Ignore(b => b.DomainEvents);
    }
}
