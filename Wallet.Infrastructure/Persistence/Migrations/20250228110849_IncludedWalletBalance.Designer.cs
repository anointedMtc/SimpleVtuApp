﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wallet.Infrastructure.Persistence;

#nullable disable

namespace Wallet.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(WalletDbContext))]
    [Migration("20250228110849_IncludedWalletBalance")]
    partial class IncludedWalletBalance
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Wallet.Domain.Entities.Owner", b =>
                {
                    b.Property<Guid>("OwnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationUserId")
                        .HasMaxLength(256)
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OwnerId");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("Wallet.Domain.Entities.WalletAggregate.Transfer", b =>
                {
                    b.Property<Guid>("TransferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ReasonWhy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WalletDomainEntityId")
                        .HasMaxLength(256)
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TransferId");

                    b.HasIndex("WalletDomainEntityId");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("Wallet.Domain.Entities.WalletAggregate.WalletDomainEntity", b =>
                {
                    b.Property<Guid>("WalletDomainEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerId")
                        .HasMaxLength(256)
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("WalletDomainEntityId");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("WalletDomainEntities");
                });

            modelBuilder.Entity("Wallet.Domain.Entities.WalletAggregate.Transfer", b =>
                {
                    b.HasOne("Wallet.Domain.Entities.WalletAggregate.WalletDomainEntity", null)
                        .WithMany("Transfers")
                        .HasForeignKey("WalletDomainEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Wallet.Domain.Entities.WalletAggregate.Amount", "Amount", b1 =>
                        {
                            b1.Property<Guid>("TransferId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal (18,2)");

                            b1.HasKey("TransferId");

                            b1.ToTable("Transfers");

                            b1.WithOwner()
                                .HasForeignKey("TransferId");
                        });

                    b.Navigation("Amount")
                        .IsRequired();
                });

            modelBuilder.Entity("Wallet.Domain.Entities.WalletAggregate.WalletDomainEntity", b =>
                {
                    b.HasOne("Wallet.Domain.Entities.Owner", "Owner")
                        .WithOne("WalletDomainEntity")
                        .HasForeignKey("Wallet.Domain.Entities.WalletAggregate.WalletDomainEntity", "OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Wallet.Domain.Entities.WalletAggregate.Amount", "WalletBalance", b1 =>
                        {
                            b1.Property<Guid>("WalletDomainEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal (18,2)");

                            b1.HasKey("WalletDomainEntityId");

                            b1.ToTable("WalletDomainEntities");

                            b1.WithOwner()
                                .HasForeignKey("WalletDomainEntityId");
                        });

                    b.Navigation("Owner");

                    b.Navigation("WalletBalance")
                        .IsRequired();
                });

            modelBuilder.Entity("Wallet.Domain.Entities.Owner", b =>
                {
                    b.Navigation("WalletDomainEntity")
                        .IsRequired();
                });

            modelBuilder.Entity("Wallet.Domain.Entities.WalletAggregate.WalletDomainEntity", b =>
                {
                    b.Navigation("Transfers");
                });
#pragma warning restore 612, 618
        }
    }
}
