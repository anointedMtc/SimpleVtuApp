using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VtuApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Changed_TimeLastStarWasAchieved_To_DateTimeOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TimeLastStarWasAchieved",
                table: "Customers",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TimeLastStarWasAchieved",
                table: "Customers",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }
    }
}
