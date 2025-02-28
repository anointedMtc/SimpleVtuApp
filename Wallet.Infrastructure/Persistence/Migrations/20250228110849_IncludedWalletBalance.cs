using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IncludedWalletBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "WalletBalance_Value",
                table: "WalletDomainEntities",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletBalance_Value",
                table: "WalletDomainEntities");
        }
    }
}
