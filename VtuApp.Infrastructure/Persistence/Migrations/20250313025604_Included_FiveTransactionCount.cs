using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VtuApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Included_FiveTransactionCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FiveTransactionCount",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FiveTransactionCount",
                table: "Customers");
        }
    }
}
