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
                name: "ApiKey",
                schema: "permissioneer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    HashedKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "permissioneer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsAssignable = table.Column<bool>(type: "bit", nullable: false)
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
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeyPermission",
                schema: "permissioneer",
                columns: table => new
                {
                    ApiKeyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeyPermission", x => new { x.ApiKeyId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_ApiKeyPermission_ApiKey_ApiKeyId",
                        column: x => x.ApiKeyId,
                        principalSchema: "permissioneer",
                        principalTable: "ApiKey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiKeyPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "permissioneer",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "permissioneer",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.PermissionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "permissioneer",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "permissioneer",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "permissioneer",
                table: "Permission",
                columns: new[] { "Id", "Description", "IsAssignable", "Name" },
                values: new object[,]
                {
                    { new Guid("05adbf0d-1b79-4777-93de-28474e9ba19e"), "Create or Update Quotes", true, "write:quotes" },
                    { new Guid("cdfd4277-e7a7-4813-9058-e109fc6a7d0c"), "Delete Quotes", true, "delete:quotes" },
                    { new Guid("d1027891-8a12-41d2-9a0d-d7b1a077a664"), "Manage All Resources", false, "manage:all-resources" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842b4"), "Read API Keys", true, "read:api-keys" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842b5"), "Create or Update API Keys", true, "write:api-keys" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842b6"), "Revoke API Keys", true, "revoke:api-keys" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c0"), "Assign Permissions", true, "assign:permissions" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c1"), "Unassign Permissions", true, "unassign:permissions" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4"), "Read Roles", true, "read:roles" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c5"), "Create or Update Roles", true, "write:roles" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c6"), "Delete Roles", true, "delete:roles" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c7"), "Read Permissions", true, "read:permissions" },
                    { new Guid("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3"), "Read Quotes", true, "read:quotes" }
                });

            migrationBuilder.InsertData(
                schema: "permissioneer",
                table: "Role",
                columns: new[] { "Id", "Description", "IsActive", "IsSystem", "Name" },
                values: new object[,]
                {
                    { new Guid("1a307ea6-fbe1-4048-a447-af7057faa5c5"), "User Role", true, true, "User" },
                    { new Guid("5c5f7695-17f7-4963-8114-526b2f024faa"), "Guest Role", true, true, "Guest" },
                    { new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4"), "Admin Role", true, true, "Admin" }
                });

            migrationBuilder.InsertData(
                schema: "permissioneer",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId", "IsAllowed", "IsSystem" },
                values: new object[,]
                {
                    { new Guid("05adbf0d-1b79-4777-93de-28474e9ba19e"), new Guid("1a307ea6-fbe1-4048-a447-af7057faa5c5"), true, true },
                    { new Guid("05adbf0d-1b79-4777-93de-28474e9ba19e"), new Guid("5c5f7695-17f7-4963-8114-526b2f024faa"), false, true },
                    { new Guid("05adbf0d-1b79-4777-93de-28474e9ba19e"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4"), true, true },
                    { new Guid("cdfd4277-e7a7-4813-9058-e109fc6a7d0c"), new Guid("5c5f7695-17f7-4963-8114-526b2f024faa"), false, true },
                    { new Guid("cdfd4277-e7a7-4813-9058-e109fc6a7d0c"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4"), true, true },
                    { new Guid("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3"), new Guid("1a307ea6-fbe1-4048-a447-af7057faa5c5"), true, true },
                    { new Guid("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3"), new Guid("5c5f7695-17f7-4963-8114-526b2f024faa"), true, true },
                    { new Guid("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3"), new Guid("f2d82c53-f6be-4095-8a98-bd62c12842c4"), true, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeyPermission_PermissionId",
                schema: "permissioneer",
                table: "ApiKeyPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                schema: "permissioneer",
                table: "RolePermission",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeyPermission",
                schema: "permissioneer");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "permissioneer");

            migrationBuilder.DropTable(
                name: "ApiKey",
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
