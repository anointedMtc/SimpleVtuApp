﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VtuApp.Infrastructure.Persistence;

#nullable disable

namespace VtuApp.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(VtuDbContext))]
    partial class VtuDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VtuApp.Domain.Entities.VtuModelAggregate.Customer", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfStars")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("TimeLastStarWasAchieved")
                        .HasColumnType("time");

                    b.Property<int>("TransactionCount")
                        .HasColumnType("int");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("VtuApp.Domain.Entities.VtuModelAggregate.VtuBonusTransfer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ReasonWhy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransferDirection")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("VtuBonusTransfers", (string)null);
                });

            modelBuilder.Entity("VtuApp.Domain.Entities.VtuModelAggregate.VtuTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NetWorkProvider")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TypeOfTransaction")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("VtuTransactions", (string)null);
                });

            modelBuilder.Entity("VtuApp.Domain.Entities.VtuModelAggregate.Customer", b =>
                {
                    b.OwnsOne("VtuApp.Domain.Entities.VtuModelAggregate.Customer.TotalBalance#VtuApp.Domain.Entities.VtuModelAggregate.VtuAmount", "TotalBalance", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal (18,2)");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("VtuApp.Domain.Entities.VtuModelAggregate.Customer.VtuBonusBalance#VtuApp.Domain.Entities.VtuModelAggregate.VtuAmount", "VtuBonusBalance", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal (18,2)");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("TotalBalance")
                        .IsRequired();

                    b.Navigation("VtuBonusBalance")
                        .IsRequired();
                });

            modelBuilder.Entity("VtuApp.Domain.Entities.VtuModelAggregate.VtuBonusTransfer", b =>
                {
                    b.HasOne("VtuApp.Domain.Entities.VtuModelAggregate.Customer", null)
                        .WithMany("VtuBonusTransfers")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("VtuApp.Domain.Entities.VtuModelAggregate.VtuBonusTransfer.AmountTransfered#VtuApp.Domain.Entities.VtuModelAggregate.VtuAmount", "AmountTransfered", b1 =>
                        {
                            b1.Property<Guid>("VtuBonusTransferId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal (18,2)");

                            b1.HasKey("VtuBonusTransferId");

                            b1.ToTable("VtuBonusTransfers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("VtuBonusTransferId");
                        });

                    b.OwnsOne("VtuApp.Domain.Entities.VtuModelAggregate.VtuBonusTransfer.FinalBalance#VtuApp.Domain.Entities.VtuModelAggregate.VtuAmount", "FinalBalance", b1 =>
                        {
                            b1.Property<Guid>("VtuBonusTransferId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal (18,2)");

                            b1.HasKey("VtuBonusTransferId");

                            b1.ToTable("VtuBonusTransfers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("VtuBonusTransferId");
                        });

                    b.OwnsOne("VtuApp.Domain.Entities.VtuModelAggregate.VtuBonusTransfer.InitialBalance#VtuApp.Domain.Entities.VtuModelAggregate.VtuAmount", "InitialBalance", b1 =>
                        {
                            b1.Property<Guid>("VtuBonusTransferId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal (18,2)");

                            b1.HasKey("VtuBonusTransferId");

                            b1.ToTable("VtuBonusTransfers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("VtuBonusTransferId");
                        });

                    b.Navigation("AmountTransfered")
                        .IsRequired();

                    b.Navigation("FinalBalance")
                        .IsRequired();

                    b.Navigation("InitialBalance")
                        .IsRequired();
                });

            modelBuilder.Entity("VtuApp.Domain.Entities.VtuModelAggregate.VtuTransaction", b =>
                {
                    b.HasOne("VtuApp.Domain.Entities.VtuModelAggregate.Customer", null)
                        .WithMany("VtuTransactions")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("VtuApp.Domain.Entities.VtuModelAggregate.VtuTransaction.Amount#VtuApp.Domain.Entities.VtuModelAggregate.VtuAmount", "Amount", b1 =>
                        {
                            b1.Property<Guid>("VtuTransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal (18,2)");

                            b1.HasKey("VtuTransactionId");

                            b1.ToTable("VtuTransactions", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("VtuTransactionId");
                        });

                    b.OwnsOne("VtuApp.Domain.Entities.VtuModelAggregate.VtuTransaction.Discount#VtuApp.Domain.Entities.VtuModelAggregate.VtuAmount", "Discount", b1 =>
                        {
                            b1.Property<Guid>("VtuTransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal (18,2)");

                            b1.HasKey("VtuTransactionId");

                            b1.ToTable("VtuTransactions", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("VtuTransactionId");
                        });

                    b.Navigation("Amount")
                        .IsRequired();

                    b.Navigation("Discount")
                        .IsRequired();
                });

            modelBuilder.Entity("VtuApp.Domain.Entities.VtuModelAggregate.Customer", b =>
                {
                    b.Navigation("VtuBonusTransfers");

                    b.Navigation("VtuTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
