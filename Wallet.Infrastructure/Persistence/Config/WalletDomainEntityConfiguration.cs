﻿using Microsoft.EntityFrameworkCore;
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

        builder.Metadata.FindNavigation(nameof(WalletDomainEntity.Transfers))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);


        builder.Ignore(b => b.DomainEvents);
    }
}
