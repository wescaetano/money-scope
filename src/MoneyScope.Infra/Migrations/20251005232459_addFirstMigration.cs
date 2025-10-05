using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoneyScope.Infra.Migrations
{
    /// <inheritdoc />
    public partial class addFirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Visualize = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Edit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Register = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Inactivate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Exclude = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExclusionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExclusionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(70)", maxLength: 70, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "0 =  Intivo,  1 = Ativo"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExclusionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProfilesModules",
                columns: table => new
                {
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Visualize = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Edit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Register = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Inactivate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Exclude = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilesModules", x => new { x.ModuleId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_ProfilesModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfilesModules_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProfilesUsers",
                columns: table => new
                {
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilesUsers", x => new { x.UserId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_ProfilesUsers_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfilesUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "CreationDate", "Edit", "Exclude", "ExclusionDate", "Inactivate", "Name", "Register", "UpdateDate", "Visualize" },
                values: new object[] { 1L, null, true, true, null, true, "Users", true, null, true });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "CreationDate", "ExclusionDate", "Name", "Status", "UpdateDate" },
                values: new object[,]
                {
                    { 1L, null, null, "Admin", 1, null },
                    { 2L, null, null, "User", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "ExclusionDate", "ImageUrl", "Name", "Password", "ProviderId", "Status", "UpdateDate" },
                values: new object[] { 1L, new DateTime(2025, 10, 5, 23, 24, 59, 357, DateTimeKind.Utc).AddTicks(1326), "admin@admin.com", null, null, "Admin", "$2a$15$5DxGdsCvuzHVigXWk8Qr1uvoizMNxrdxz6SypelRVxC7n1D9uHB7.", null, 1, null });

            migrationBuilder.InsertData(
                table: "ProfilesModules",
                columns: new[] { "ModuleId", "ProfileId", "Edit", "Exclude", "Inactivate", "Register", "Visualize" },
                values: new object[,]
                {
                    { 1L, 1L, true, true, true, true, true },
                    { 1L, 2L, true, false, true, true, true }
                });

            migrationBuilder.InsertData(
                table: "ProfilesUsers",
                columns: new[] { "ProfileId", "UserId" },
                values: new object[] { 1L, 1L });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilesModules_ProfileId",
                table: "ProfilesModules",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilesUsers_ProfileId",
                table: "ProfilesUsers",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilesModules");

            migrationBuilder.DropTable(
                name: "ProfilesUsers");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
