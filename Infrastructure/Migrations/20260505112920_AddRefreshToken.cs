using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 5, 11, 29, 20, 25, DateTimeKind.Utc).AddTicks(3311));

            migrationBuilder.UpdateData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("b4dcb17c-f875-43f8-9d66-36597895a466"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 5, 11, 29, 20, 25, DateTimeKind.Utc).AddTicks(3320));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 8, 53, 26, 411, DateTimeKind.Utc).AddTicks(838));

            migrationBuilder.UpdateData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("b4dcb17c-f875-43f8-9d66-36597895a466"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 8, 53, 26, 411, DateTimeKind.Utc).AddTicks(850));
        }
    }
}
