using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenDulu.BE.Migrations
{
    /// <inheritdoc />
    public partial class migrate13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "employee_shift",
                table: "attendance",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "employee_shift",
                table: "attendance");
        }
    }
}
