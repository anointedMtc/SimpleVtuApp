using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VtuApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VtuBonusBalance_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBalance_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfStars = table.Column<int>(type: "int", nullable: false),
                    TransactionCount = table.Column<int>(type: "int", nullable: false),
                    TimeLastStarWasAchieved = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "VtuBonusTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountTransfered_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InitialBalance_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalBalance_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransferDirection = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ReasonWhy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VtuBonusTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VtuBonusTransfers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VtuTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeOfTransaction = table.Column<int>(type: "int", nullable: false),
                    NetWorkProvider = table.Column<int>(type: "int", nullable: false),
                    Amount_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Discount_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VtuTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VtuTransactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VtuBonusTransfers_CustomerId",
                table: "VtuBonusTransfers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_VtuTransactions_CustomerId",
                table: "VtuTransactions",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VtuBonusTransfers");

            migrationBuilder.DropTable(
                name: "VtuTransactions");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
