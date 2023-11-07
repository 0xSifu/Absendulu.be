using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenDulu.BE.Migrations
{
    /// <inheritdoc />
    public partial class migrate10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "master_onleave");

            migrationBuilder.CreateTable(
                name: "master_leave",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    leave_code = table.Column<string>(type: "text", nullable: true),
                    leave_name = table.Column<string>(type: "text", nullable: true),
                    total_days = table.Column<int>(type: "integer", nullable: true),
                    leave_more_than_balance = table.Column<bool>(type: "boolean", nullable: true),
                    company = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_leave", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "master_reimbusment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reimbusment_code = table.Column<string>(type: "text", nullable: true),
                    reimbusment_name = table.Column<string>(type: "text", nullable: true),
                    total_amount = table.Column<string>(type: "text", nullable: true),
                    company = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_reimbusment", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "master_leave");

            migrationBuilder.DropTable(
                name: "master_reimbusment");

            migrationBuilder.CreateTable(
                name: "master_onleave",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    onleave_code = table.Column<string>(type: "text", nullable: true),
                    onleave_name = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_onleave", x => x.id);
                });
        }
    }
}
