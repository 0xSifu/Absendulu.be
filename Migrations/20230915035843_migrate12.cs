using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenDulu.BE.Migrations
{
    /// <inheritdoc />
    public partial class migrate12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attendance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    attendance_method = table.Column<string>(type: "text", nullable: true),
                    attendance_type = table.Column<string>(type: "text", nullable: true),
                    attendance_image = table.Column<string>(type: "text", nullable: true),
                    latitude = table.Column<string>(type: "text", nullable: true),
                    longitude = table.Column<string>(type: "text", nullable: true),
                    location_address = table.Column<string>(type: "text", nullable: true),
                    is_approved = table.Column<bool>(type: "boolean", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    employee_id = table.Column<string>(type: "text", nullable: true),
                    company = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendance");
        }
    }
}
