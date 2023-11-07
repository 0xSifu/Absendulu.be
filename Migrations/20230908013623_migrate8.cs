using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenDulu.BE.Migrations
{
    /// <inheritdoc />
    public partial class migrate8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ip_address_user_login",
                table: "user_account",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_login",
                table: "user_account",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ip_address_user_login",
                table: "user_account");

            migrationBuilder.DropColumn(
                name: "last_login",
                table: "user_account");
        }
    }
}
