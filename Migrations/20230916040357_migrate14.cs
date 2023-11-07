using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenDulu.BE.Migrations
{
    /// <inheritdoc />
    public partial class migrate14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "company_id",
                table: "attendance",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "view_attendances",
                columns: table => new
                {
                    employee_name = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    clock_in = table.Column<string>(type: "text", nullable: false),
                    clock_in_method = table.Column<string>(type: "text", nullable: false),
                    clock_in_address = table.Column<string>(type: "text", nullable: false),
                    clock_in_note = table.Column<string>(type: "text", nullable: false),
                    clockin_photo = table.Column<string>(type: "text", nullable: false),
                    clock_out = table.Column<string>(type: "text", nullable: false),
                    clock_out_method = table.Column<string>(type: "text", nullable: false),
                    clock_out_address = table.Column<string>(type: "text", nullable: false),
                    clock_out_note = table.Column<string>(type: "text", nullable: false),
                    clockout_photo = table.Column<string>(type: "text", nullable: false),
                    department_name = table.Column<string>(type: "text", nullable: false),
                    employee_shift = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "view_attendances");

            migrationBuilder.AlterColumn<string>(
                name: "company_id",
                table: "attendance",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
