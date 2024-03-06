namespace Dabitco.Permissioneer.TestAPI;

using Dabitco.Permissioneer.Domain.Models;

public static class PermissioneerSeedData
{
    public static IEnumerable<PermissionSeedData> PermissionsSeed =>
    [
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsIds.QuotesRead,
            Name = "read:quotes",
            Description = "Read Quotes"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsIds.QuotesWrite,
            Name = "write:quotes",
            Description = "Create or Update Quotes"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsIds.QuotesDelete,
            Name = "delete:quotes",
            Description = "Delete Quotes"
        },
        new PermissionSeedData
        {
            Id = PermissioneerSeedDataPermissionsIds.ManageAllResources,
            Name = "manage:all-resources",
            Description = "Manage All Resources",
            IsAssignable = false
        }
    ];

    public static IEnumerable<RoleSeedData> RolesSeed =>
    [
        new RoleSeedData
        {
            Id = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842c4"),
            Name = "Admin",
            Description = "Admin Role",
            PermissionsAllowedIds =
            [
                PermissioneerSeedDataPermissionsIds.QuotesRead,
                PermissioneerSeedDataPermissionsIds.QuotesWrite,
                PermissioneerSeedDataPermissionsIds.QuotesDelete,
            ]
        },
        new RoleSeedData
        {
            Id = Guid.Parse("1a307ea6-fbe1-4048-a447-af7057faa5c5"),
            Name = "User",
            Description = "User Role",
            PermissionsAllowedIds =
            [
                PermissioneerSeedDataPermissionsIds.QuotesRead,
                PermissioneerSeedDataPermissionsIds.QuotesWrite
            ]
        },
        new RoleSeedData
        {
            Id = Guid.Parse("5c5f7695-17f7-4963-8114-526b2f024faa"),
            Name = "Guest",
            Description = "Guest Role",
            PermissionsAllowedIds =
            [
                PermissioneerSeedDataPermissionsIds.QuotesRead
            ],
            PermissionsDeniedIds =
            [
                PermissioneerSeedDataPermissionsIds.QuotesWrite,
                PermissioneerSeedDataPermissionsIds.QuotesDelete
            ]
        }
    ];
}


public static class PermissioneerSeedDataPermissionsIds
{
    public static Guid QuotesRead => Guid.Parse("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3");
    public static Guid QuotesWrite => Guid.Parse("05adbf0d-1b79-4777-93de-28474e9ba19e");
    public static Guid QuotesDelete => Guid.Parse("cdfd4277-e7a7-4813-9058-e109fc6a7d0c");
    public static Guid ManageAllResources => Guid.Parse("d1027891-8a12-41d2-9a0d-d7b1a077a664");
}
