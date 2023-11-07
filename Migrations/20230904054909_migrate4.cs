using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenDulu.BE.Migrations
{
    /// <inheritdoc />
    public partial class migrate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "company_code",
                table: "employees",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "remaining_apps",
                table: "details_subcribe",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "remaining_device",
                table: "details_subcribe",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "remaining_apps",
                table: "details_subcribe");

            migrationBuilder.DropColumn(
                name: "remaining_device",
                table: "details_subcribe");

            migrationBuilder.AlterColumn<string>(
                name: "company_code",
                table: "employees",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
