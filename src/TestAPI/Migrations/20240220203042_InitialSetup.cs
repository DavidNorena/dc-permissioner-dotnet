using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dabitco.Permissioneer.TestAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "permissioneer");

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "permissioneer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "permissioneer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionAllowed",
                schema: "permissioneer",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionAllowed", x => new { x.PermissionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RolePermissionAllowed_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "permissioneer",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionAllowed_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "permissioneer",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionDenied",
                schema: "permissioneer",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionDenied", x => new { x.PermissionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RolePermissionDenied_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "permissioneer",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionDenied_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "permissioneer",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "permissioneer",
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("05adbf0d-1b79-4777-93de-28474e9ba19e"), "Permissioneer.Roles.Write" },
                    { new Guid("19c407cf-cf12-4b81-8552-e985398ce50d"), "Permissioneer.Permissions.Assign" },
                    { new Guid("2069215b-f033-4a43-a8c7-09594a5e191b"), "Permissioneer.Permissions.Write" },
                    { new Guid("2552f6e4-a731-4428-bca5-816d4d00b3f9"), "Permissioneer.Permissions.Unassign" },
                    { new Guid("5dace4cf-a662-4192-bd7b-3bc30a06f4c0"), "Permissioneer.Permissions.Delete" },
                    { new Guid("cdfd4277-e7a7-4813-9058-e109fc6a7d0c"), "Permissioneer.Roles.Delete" },
                    { new Guid("d6372504-f1f8-4c41-8b1c-7d62f181c92d"), "Permissioneer.Permissions.Read" },
                    { new Guid("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3"), "Permissioneer.Roles.Read" }
                });

            migrationBuilder.InsertData(
                schema: "permissioneer",
                table: "Role",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("1a307ea6-fbe1-4048-a447-af7057faa5c5"), true, "User" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4"), true, "Admin" }
                });

            migrationBuilder.InsertData(
                schema: "permissioneer",
                table: "RolePermissionAllowed",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("05adbf0d-1b79-4777-93de-28474e9ba19e"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4") },
                    { new Guid("19c407cf-cf12-4b81-8552-e985398ce50d"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4") },
                    { new Guid("2069215b-f033-4a43-a8c7-09594a5e191b"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4") },
                    { new Guid("2552f6e4-a731-4428-bca5-816d4d00b3f9"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4") },
                    { new Guid("5dace4cf-a662-4192-bd7b-3bc30a06f4c0"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4") },
                    { new Guid("cdfd4277-e7a7-4813-9058-e109fc6a7d0c"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4") },
                    { new Guid("d6372504-f1f8-4c41-8b1c-7d62f181c92d"), new Guid("1a307ea6-fbe1-4048-a447-af7057faa5c5") },
                    { new Guid("d6372504-f1f8-4c41-8b1c-7d62f181c92d"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4") },
                    { new Guid("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3"), new Guid("1a307ea6-fbe1-4048-a447-af7057faa5c5") },
                    { new Guid("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4") }
                });

            migrationBuilder.InsertData(
                schema: "permissioneer",
                table: "RolePermissionDenied",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { new Guid("cdfd4277-e7a7-4813-9058-e109fc6a7d0c"), new Guid("1a307ea6-fbe1-4048-a447-af7057faa5c5") });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionAllowed_RoleId",
                schema: "permissioneer",
                table: "RolePermissionAllowed",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionDenied_RoleId",
                schema: "permissioneer",
                table: "RolePermissionDenied",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissionAllowed",
                schema: "permissioneer");

            migrationBuilder.DropTable(
                name: "RolePermissionDenied",
                schema: "permissioneer");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "permissioneer");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "permissioneer");
        }
    }
}
