namespace Dabitco.Permissioneer.EntityFramework.Services;

using Dabitco.Permissioneer.Domain.Abstract.Storage;
using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class DbApiKeysStorage(PermissioneerDbContext dbContext, PermissionsStorageBase permissionsStorage) : ApiKeysStorageBase
{
    public override async Task<string> AddApiKeyAsync(ApiKeyAddRequest addRequest)
    {
        var permissions = await permissionsStorage.GetPermissionsAsync(addRequest.PermissionsIds.ToArray());

        var apiKey = GenerateSecureApiKey();
        var hashedApiKey = HashApiKey(apiKey);

        var apiKeyEntity = new ApiKeyEntity
        {
            Id = Guid.NewGuid(),
            Name = addRequest.Name,
            Description = addRequest.Description,
            HashedKey = hashedApiKey,
            CreatedDate = DateTime.UtcNow,
            ExpirationDate = addRequest.ExpirationDate,
            OwnerId = addRequest.OwnerId,
            Permissions = permissions.ToList(),
        };

        await dbContext.ApiKeys.AddAsync(apiKeyEntity);
        await dbContext.SaveChangesAsync();

        return apiKey;
    }

    public override Task<ApiKeyModel?> GetApiKeyAsync(string apiKey)
    {
        var apiKeyEntity = dbContext.ApiKeys
            .Include(a => a.Permissions)
            .FirstOrDefault(a => a.HashedKey == HashApiKey(apiKey) && (a.ExpirationDate == null || a.ExpirationDate > DateTime.UtcNow));

        var result = apiKeyEntity is null
            ? null
            : new ApiKeyModel
            {
                Id = apiKeyEntity.Id,
                Name = apiKeyEntity.Name,
                Description = apiKeyEntity.Description,
                CreatedDate = apiKeyEntity.CreatedDate,
                ExpirationDate = apiKeyEntity.ExpirationDate,
                OwnerId = apiKeyEntity.OwnerId,
                Permissions = apiKeyEntity.Permissions.Select(p => p.Name),
            };

        return Task.FromResult(result);
    }

    public override async Task<IEnumerable<ApiKeyModel>> ListApiKeysAsync(string? ownerId = null)
    {
        var apiKeys = dbContext.ApiKeys
            .Include(a => a.Permissions).AsQueryable();

        if (ownerId != null)
        {
            apiKeys = apiKeys.Where(a => a.OwnerId == ownerId);
        }

        var result = apiKeys.Select(a => new ApiKeyModel
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            CreatedDate = a.CreatedDate,
            ExpirationDate = a.ExpirationDate,
            OwnerId = a.OwnerId,
            Permissions = a.Permissions.Select(p => p.Name),
        });

        return await result.ToListAsync();
    }

    public override async Task RevokeApiKeyAsync(Guid apiKeyId)
    {
        var apiKey = await dbContext.ApiKeys
            .Include(a => a.Permissions)
            .FirstOrDefaultAsync(a => a.Id == apiKeyId)
                ?? throw new InvalidOperationException("API key with does not exist");

        dbContext.ApiKeys.Remove(apiKey);
        await dbContext.SaveChangesAsync();
    }
}