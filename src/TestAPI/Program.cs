using System.Reflection;
using Dabitco.Permissioneer.Domain;
using Microsoft.OpenApi.Models;
using Dabitco.Permissioneer.TestAPI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Permissioneer Test Service API",
        Contact = new OpenApiContact
        {
            Name = "devs@dabit.co",
        },
    });
});

builder.Services.Configure<RouteOptions>(opts => opts.LowercaseUrls = true);
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddLogging();
var permissioneerBuilder = builder.Services.AddPermissioneer()
    // UNCOMMENT TO USE JUST IN-MEMORY STORAGE
    // .AddInMemoryStorage();
    // COMMENT OUT TO USE JUST IN-MEMORY STORAGE
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

if (builder.Environment.IsDevelopment())
{
    permissioneerBuilder
        .AddPermissionsSeedData(PermissioneerSeedData.PermissionsSeed)
        .AddRolesSeedData(PermissioneerSeedData.RolesSeed);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapControllers();

app.Run();
