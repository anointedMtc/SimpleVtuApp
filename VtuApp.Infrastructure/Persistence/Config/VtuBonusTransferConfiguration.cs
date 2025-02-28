using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Infrastructure.Persistence.Config;

public class VtuBonusTransferConfiguration : IEntityTypeConfiguration<VtuBonusTransfer>
{
    public void Configure(EntityTypeBuilder<VtuBonusTransfer> builder)
    {
        builder.HasKey(b => b.Id);


        builder.OwnsOne(p => p.AmountTransfered, p =>
        {
            p.Property(pp => pp.Value).IsRequired().HasColumnType("decimal (18,2)");
        });

        builder.OwnsOne(p => p.InitialBalance, p =>
        {
            p.Property(pp => pp.Value).IsRequired().HasColumnType("decimal (18,2)");
        });

        builder.OwnsOne(p => p.FinalBalance, p =>
        {
            p.Property(pp => pp.Value).IsRequired().HasColumnType("decimal (18,2)");
        });

        builder.Property(b => b.TransferDirection)
         .IsRequired()
         .HasConversion<string>()
         .HasMaxLength(30);

        builder.Ignore(b => b.DomainEvents);
    }
}
