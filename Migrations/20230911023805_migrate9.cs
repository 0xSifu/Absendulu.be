using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenDulu.BE.Migrations
{
    /// <inheritdoc />
    public partial class migrate9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "master_shift",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    shift_code = table.Column<string>(type: "text", nullable: true),
                    shift_name = table.Column<string>(type: "text", nullable: true),
                    start_work_time = table.Column<string>(type: "text", nullable: true),
                    end_work_time = table.Column<string>(type: "text", nullable: true),
                    start_break_time = table.Column<string>(type: "text", nullable: true),
                    end_break_time = table.Column<string>(type: "text", nullable: true),
                    work_days = table.Column<string[]>(type: "text[]", nullable: true),
                    company = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_shift", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "master_shift");
        }
    }
}
