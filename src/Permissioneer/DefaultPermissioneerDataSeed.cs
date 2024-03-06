namespace Dabitco.Permissioneer;

using Dabitco.Permissioneer.Domain.Models;

public static class DefaultPermissioneerDataSeed
{
    public static IEnumerable<PermissionSeedData> PermissionsSeed =>
    [
        new PermissionSeedData
        {
            Id = DefaultPermissioneerDataSeedPermissionsIds.ApiKeysRead,
            Name = "read:api-keys",
            Description = "Read API Keys"
        },
        new PermissionSeedData
        {
            Id = DefaultPermissioneerDataSeedPermissionsIds.ApiKeysWrite,
            Name = "write:api-keys",
            Description = "Create or Update API Keys"
        },
        new PermissionSeedData
        {
            Id = DefaultPermissioneerDataSeedPermissionsIds.ApiKeysRevoke,
            Name = "revoke:api-keys",
            Description = "Revoke API Keys"
        },
        new PermissionSeedData
        {
            Id = DefaultPermissioneerDataSeedPermissionsIds.RolesRead,
            Name = "read:roles",
            Description = "Read Roles"
        },
        new PermissionSeedData
        {
            Id = DefaultPermissioneerDataSeedPermissionsIds.RolesWrite,
            Name = "write:roles",
            Description = "Create or Update Roles"
        },
        new PermissionSeedData
        {
            Id = DefaultPermissioneerDataSeedPermissionsIds.RolesDelete,
            Name = "delete:roles",
            Description = "Delete Roles"
        },
        new PermissionSeedData
        {
            Id = DefaultPermissioneerDataSeedPermissionsIds.PermissionsRead,
            Name = "read:permissions",
            Description = "Read Permissions"
        },
        new PermissionSeedData
        {
            Id = DefaultPermissioneerDataSeedPermissionsIds.PermissionsAssign,
            Name = "assign:permissions",
            Description = "Assign Permissions"
        },
        new PermissionSeedData
        {
            Id = DefaultPermissioneerDataSeedPermissionsIds.PermissionsUnassign,
            Name = "unassign:permissions",
            Description = "Unassign Permissions"
        }
    ];
}

public static class DefaultPermissioneerDataSeedPermissionsIds
{
    public static readonly Guid ApiKeysRead = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842b4");
    public static readonly Guid ApiKeysWrite = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842b5");
    public static readonly Guid ApiKeysRevoke = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842b6");
    public static readonly Guid RolesRead = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842c4");
    public static readonly Guid RolesWrite = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842c5");
    public static readonly Guid RolesDelete = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842c6");
    public static readonly Guid PermissionsRead = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842c7");
    public static readonly Guid PermissionsAssign = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842c0");
    public static readonly Guid PermissionsUnassign = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842c1");
}
