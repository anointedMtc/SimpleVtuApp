using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VtuApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Renamed_TotalBalance_To_MainBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalBalance_Value",
                table: "Customers",
                newName: "MainBalance_Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MainBalance_Value",
                table: "Customers",
                newName: "TotalBalance_Value");
        }
    }
}
