namespace Dabitco.Permissioneer.TestAPI;

using Dabitco.Permissioneer.Domain.Models;

public static class PermissioneerSeedData
{
    public static IEnumerable<PermissionSeedData> PermissionsSeed =>
    [
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsId.RolesRead,
            Name = "Permissioneer.Roles.Read"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsId.RolesWrite,
            Name = "Permissioneer.Roles.Write"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsId.RolesDelete,
            Name = "Permissioneer.Roles.Delete"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsId.PermissionsRead,
            Name = "Permissioneer.Permissions.Read"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsId.PermissionsWrite,
            Name = "Permissioneer.Permissions.Write"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsId.PermissionsDelete,
            Name = "Permissioneer.Permissions.Delete"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsId.PermissionsAssign,
            Name = "Permissioneer.Permissions.Assign"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsId.PermissionsUnassign,
            Name = "Permissioneer.Permissions.Unassign"
        }
    ];

    public static IEnumerable<RoleSeedData> RolesSeed =>
    [
        new RoleSeedData
        {
            Id = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842c4"),
            Name = "Admin",
            PermissionsAllowedIds =
            [
                PermissioneerSeedDataPermissionsId.RolesRead,
                PermissioneerSeedDataPermissionsId.RolesWrite,
                PermissioneerSeedDataPermissionsId.RolesDelete,
                PermissioneerSeedDataPermissionsId.PermissionsRead,
                PermissioneerSeedDataPermissionsId.PermissionsWrite,
                PermissioneerSeedDataPermissionsId.PermissionsDelete,
                PermissioneerSeedDataPermissionsId.PermissionsAssign,
                PermissioneerSeedDataPermissionsId.PermissionsUnassign
            ]
        },
        new RoleSeedData
        {
            Id = Guid.Parse("1a307ea6-fbe1-4048-a447-af7057faa5c5"),
            Name = "User",
            PermissionsAllowedIds =
            [
                PermissioneerSeedDataPermissionsId.RolesRead,
                PermissioneerSeedDataPermissionsId.PermissionsRead
            ],
            PermissionsDeniedIds =
            [
                PermissioneerSeedDataPermissionsId.RolesDelete,
            ]
        }
    ];
}


public static class PermissioneerSeedDataPermissionsId
{
    public static Guid RolesRead => Guid.Parse("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3");
    public static Guid RolesWrite => Guid.Parse("05adbf0d-1b79-4777-93de-28474e9ba19e");
    public static Guid RolesDelete => Guid.Parse("cdfd4277-e7a7-4813-9058-e109fc6a7d0c");
    public static Guid PermissionsRead => Guid.Parse("d6372504-f1f8-4c41-8b1c-7d62f181c92d");
    public static Guid PermissionsWrite => Guid.Parse("2069215b-f033-4a43-a8c7-09594a5e191b");
    public static Guid PermissionsDelete => Guid.Parse("5dace4cf-a662-4192-bd7b-3bc30a06f4c0");
    public static Guid PermissionsAssign => Guid.Parse("19c407cf-cf12-4b81-8552-e985398ce50d");
    public static Guid PermissionsUnassign => Guid.Parse("2552f6e4-a731-4428-bca5-816d4d00b3f9");
}
