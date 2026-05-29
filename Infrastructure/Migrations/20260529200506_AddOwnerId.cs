using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Contact",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                columns: new[] { "CreatedAt", "OwnerId" },
                values: new object[] { new DateTime(2026, 5, 29, 20, 5, 4, 664, DateTimeKind.Utc).AddTicks(3802), "F5BADE14-6CC8-42A2-9A44-9842DA2D9280" });

            migrationBuilder.UpdateData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("516a34d7-ccfb-4f20-85f3-62bd0f3af271"),
                column: "OwnerId",
                value: "F5BADE14-6CC8-42A2-9A44-9842DA2D9280");

            migrationBuilder.UpdateData(
                table: "Contact",
                keyColumn: "Id",
                keyValue: new Guid("b4dcb17c-f875-43f8-9d66-36597895a466"),
                columns: new[] { "CreatedAt", "OwnerId" },
                values: new object[] { new DateTime(2026, 5, 29, 20, 5, 4, 664, DateTimeKind.Utc).AddTicks(3810), "93A7FFDD-057F-4021-9C68-FE06951FFA65" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Contact");

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
    }
}
