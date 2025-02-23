using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Infrastructure.Persistence.Config;

internal class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(b => b.OwnerId);

        builder.Property(b => b.ApplicationUserId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(b => b.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(b => b.FirstName)
            .IsRequired()
            .HasMaxLength(256);

        // NON-CLUSTERED
        //builder.HasIndex(b => b.Email)
        //     .IsUnique();

        //builder.HasOne(x => x.WalletDomainEntity)
        //       .WithOne()
        //       .HasForeignKey("WalletId");

        //builder.HasOne(x => x.WalletDomainEntity)
        //       .WithOne()
        //       .HasForeignKey(nameof(Owner.WalletId));

        //builder.HasOne(x => x.WalletDomainEntity)
        //    .WithOne(x => x.Owner)
        //    .HasForeignKey<string>(nameof(Owner.OwnerId))
        //    .IsRequired();

        /*
           modelBuilder.Entity<Owner>()
            .HasOne(e => e.WalletDomainEntity)
            .WithOne(e => e.Owner)
            .HasForeignKey<WalletDomainEntity>(e => e.OwnerId)
            .IsRequired();
         */

        builder.HasOne(x => x.WalletDomainEntity)
            .WithOne(x => x.Owner)
            .HasForeignKey<WalletDomainEntity>(e => e.OwnerId)
            .IsRequired();

        builder.Ignore(b => b.DomainEvents);
    }
}



