using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sisa.Identity.DbMigrator.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionsToRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "permissions",
                table: "roles",
                type: "text[]",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "settings",
                table: "applications",
                type: "text",
                nullable: true,
                defaultValueSql: "'{}'",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "json_web_key_set",
                table: "applications",
                type: "text",
                nullable: true,
                defaultValueSql: "'{}'",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "permissions",
                table: "roles");

            migrationBuilder.AlterColumn<string>(
                name: "settings",
                table: "applications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValueSql: "'{}'");

            migrationBuilder.AlterColumn<string>(
                name: "json_web_key_set",
                table: "applications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValueSql: "'{}'");
        }
    }
}
