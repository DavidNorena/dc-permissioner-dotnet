# Permissioneer Library

The Permissioneer Library is a flexible and easy-to-use .NET library designed to manage roles and permissions within your application. It supports in-memory and Entity Framework storage out of the box, with an easy setup process for integrating role-based access control into your projects.

## Features

- Role and permission management
- In-memory or Entity Framework storage
- Easy integration with .NET projects
- Seed data for quick setup

## Getting Started

### Installation

Adding Permissioneer to your .NET project is straightforward with either the NuGet Package Manager or the .NET Core CLI. Follow these steps to get started:

#### Basic Installation

For the core functionality, which includes in-memory support for roles and permissions management, use the following command:

```csharp
dotnet add package Dabitco.Permissioneer
```

This command adds the Permissioneer library to your project, enabling you to manage roles and permissions in memory.

#### EntityFramework Support

If your project requires integration with EntityFramework for persistent storage of roles and permissions, you'll need to install an additional package. This allows Permissioneer to work with a database using EntityFramework, providing a more scalable and permanent solution for role and permission management.

To add EntityFramework support, run:

```csharp
dotnet add package Dabitco.Permissioneer.EntityFramework
```

After installing this package, you can configure Permissioneer to use EntityFramework as its storage mechanism, leveraging all the benefits of a database-backed storage system.

### Configuration

Successfully integrate Permissioneer into your .NET application by configuring services within the `Program.cs` file or in any appropriate configuration file where services are set up.

#### Basic Setup

To get started with Permissioneer using in-memory storage (ideal for development environments or applications with basic permissions requirements), add the following service configuration:

```csharp
var permissioneerBuilder = builder.Services.AddPermissioneer()
    .AddInMemoryStorage();
```

This setup initializes Permissioneer with an in-memory store for managing roles and permissions, providing a quick and easy way to start working with the library without needing a database.

#### Entity Framework Storage (Optional)

For applications requiring persistent storage, Permissioneer offers integration with Entity Framework. This option allows storing roles and permissions in a relational database, offering enhanced scalability and data persistence.

Configure Entity Framework storage by adding the following to your service configuration:

```csharp
var permissioneerBuilder = builder.Services.AddPermissioneer()
    .AddEntityFrameworkStorage(opts =>
    {
        var conStrBuilder = new SqlConnectionStringBuilder(
            builder.Configuration.GetConnectionString("PermissioneerDbConnection")
        )
        {
            Password = builder.Configuration["PermissioneerDbPassword"]
        };

        var defaultSchema = "permissioneer";

        opts.DefaultSchema = defaultSchema;
        opts.ConfigureDbContext = b => b.UseSqlServer(conStrBuilder.ConnectionString,
            b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
                    .MigrationsHistoryTable("__EFMigrationsHistory", defaultSchema));
    });
```

**Note**: Adjusting the default schema is an advanced option that may be necessary depending on your application's database schema requirements. This setting is optional and can be tailored to fit your needs. Additionally, the `PermissioneerDbPassword` should be securely managed, preferably set as a user secret in development environments using the command `dotnet user-secrets set "PermissioneerDbPassword" "YourSecurePassword"`.

This enhanced configuration enables the use of Entity Framework for a robust, production-ready storage solution for your roles and permissions management.

### Seed Data

Seed data in Permissioneer serves two key purposes. For development or testing, especially with in-memory storage, it provides an initial configuration of roles and permissions, facilitating quick setup and testing. In production environments using Entity Framework, seed data can be part of database migrations, ensuring consistent configurations across all deployment stages and simplifying version control and updates.

To ensure performance and prevent accidental data seeding in production, seed data configuration is recommended solely for development environments. This practice safeguards against unintended modifications to production databases and maintains the integrity of migrations. Apply seed data in development as follows:

```csharp
if (builder.Environment.IsDevelopment())
{
    permissioneerBuilder
        .AddPermissionsSeedData(PermissioneerSeedData.PermissionsSeed)
        .AddRolesSeedData(PermissioneerSeedData.RolesSeed);
}
```

This conditional setup ensures a seamless transition from development to production, keeping your application's performance optimized and your data consistent.

### Usage

After setting up Permissioneer, you can use it to manage roles and permissions in your application. The library provides APIs to create, read, update, and delete roles and permissions, and to assign permissions to roles.

## Seed Data

The Permissioneer library includes a mechanism for seeding your application with initial roles and permissions. This feature is particularly useful for setting up a consistent development environment or preparing your application for production with a predefined set of access controls.

Here's a basic example of how you might define seed data for permissions and roles:

```csharp
using Dabitco.Permissioneer.Domain.Models;

public static class PermissioneerSeedData
{
    public static IEnumerable<PermissionSeedData> PermissionsSeed =>
    [
        new PermissionSeedData { Id = PermissioneerSeedDataPermissionsId.RolesRead, Name = "Permissioneer.Roles.Read" },
        new PermissionSeedData { Id = PermissioneerSeedDataPermissionsId.RolesWrite, Name = "Permissioneer.Roles.Write" },
        // Additional permissions omitted for brevity
    ];

    public static IEnumerable<RoleSeedData> RolesSeed =>
    [
        new RoleSeedData
        {
            Id = Guid.Parse("f2d82c53-f6be-4095-8a98-bd62c12842c4"),
            Name = "Admin",
            PermissionsAllowedIds = new[]
            {
                PermissioneerSeedDataPermissionsId.RolesRead,
                PermissioneerSeedDataPermissionsId.RolesWrite,
                // Additional permissions omitted for brevity
            }
        },
        // Additional roles omitted for brevity
    ];
}

public static class PermissioneerSeedDataPermissionsId
{
    public static Guid RolesRead = Guid.Parse("f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3");
    public static Guid RolesWrite = Guid.Parse("05adbf0d-1b79-4777-93de-28474e9ba19e");
    // Additional GUIDs omitted for brevity
}
```

### Understanding GUIDs

In the seed data, each permission and role is identified by a unique GUID (Globally Unique Identifier). These identifiers ensure that each entity (permission or role) is unique across the database. For example:

- `RolesRead` (f9ec9c70-3c35-4b6d-b82a-5bbd4b43e4a3) represents the permission to read roles within the system.
- `RolesWrite` (05adbf0d-1b79-4777-93de-28474e9ba19e) enables the permission to create or modify roles.

These GUIDs are used to reference permissions when assigning them to roles, ensuring a clear and unambiguous definition of access controls within your application.
