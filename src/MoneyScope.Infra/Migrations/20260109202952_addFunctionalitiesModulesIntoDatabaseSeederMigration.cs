using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoneyScope.Infra.Migrations
{
    /// <inheritdoc />
    public partial class addFunctionalitiesModulesIntoDatabaseSeederMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "CreationDate", "Edit", "Exclude", "ExclusionDate", "Inactivate", "Name", "Register", "UpdateDate", "Visualize" },
                values: new object[,]
                {
                    { 2L, null, true, true, null, true, "Auth", true, null, true },
                    { 3L, null, true, true, null, true, "Goals", true, null, true },
                    { 4L, null, true, true, null, true, "Reports", true, null, true },
                    { 5L, null, true, true, null, true, "SendEmail", true, null, true },
                    { 6L, null, true, true, null, true, "Transactions", true, null, true },
                    { 7L, null, true, true, null, true, "TransactionCategories", true, null, true }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 9, 20, 29, 52, 236, DateTimeKind.Utc).AddTicks(2486));

            migrationBuilder.InsertData(
                table: "ProfilesModules",
                columns: new[] { "ModuleId", "ProfileId", "Edit", "Exclude", "Inactivate", "Register", "Visualize" },
                values: new object[,]
                {
                    { 2L, 1L, true, true, true, true, true },
                    { 2L, 2L, true, false, false, true, true },
                    { 3L, 1L, true, true, true, true, true },
                    { 3L, 2L, true, false, true, true, true },
                    { 4L, 1L, true, true, true, true, true },
                    { 4L, 2L, true, false, true, true, true },
                    { 5L, 1L, true, true, true, true, true },
                    { 5L, 2L, true, false, true, true, true },
                    { 6L, 1L, true, true, true, true, true },
                    { 6L, 2L, true, false, true, true, true },
                    { 7L, 1L, true, true, true, true, true },
                    { 7L, 2L, true, false, true, true, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 2L, 1L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 2L, 2L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 3L, 1L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 3L, 2L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 4L, 1L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 4L, 2L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 5L, 1L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 5L, 2L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 6L, 1L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 6L, 2L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 7L, 1L });

            migrationBuilder.DeleteData(
                table: "ProfilesModules",
                keyColumns: new[] { "ModuleId", "ProfileId" },
                keyValues: new object[] { 7L, 2L });

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 31, 18, 24, 43, 664, DateTimeKind.Utc).AddTicks(12));
        }
    }
}
