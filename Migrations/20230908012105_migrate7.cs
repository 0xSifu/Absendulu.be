using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenDulu.BE.Migrations
{
    /// <inheritdoc />
    public partial class migrate7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "is_used",
                table: "user_account",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_used",
                table: "user_account");
        }
    }
}
