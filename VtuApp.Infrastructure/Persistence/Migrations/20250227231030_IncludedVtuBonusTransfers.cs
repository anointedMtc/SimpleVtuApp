using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VtuApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IncludedVtuBonusTransfers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BonusBalance_Value",
                table: "Customers",
                newName: "VtuBonusBalance_Value");

            migrationBuilder.CreateTable(
                name: "VtuBonusTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountTransfered_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InitialBalance_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalBalance_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_VtuBonusTransfers_CustomerId",
                table: "VtuBonusTransfers",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VtuBonusTransfers");

            migrationBuilder.RenameColumn(
                name: "VtuBonusBalance_Value",
                table: "Customers",
                newName: "BonusBalance_Value");
        }
    }
}
