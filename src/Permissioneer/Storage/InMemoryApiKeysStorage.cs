namespace Dabitco.Permissioneer.Storage;

using Dabitco.Permissioneer.Domain.Abstract.Storage;
using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;

public class InMemoryApiKeysStorage(PermissionsStorageBase permissionsStorage) : ApiKeysStorageBase
{
    private readonly List<ApiKeyEntity> apiKeys = [];

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

        apiKeys.Add(apiKeyEntity);

        return apiKey;
    }

    public override Task<ApiKeyModel?> GetApiKeyAsync(string apiKey)
    {
        var apiKeyEntity = apiKeys
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

    public override Task<ApiKeyModel?> GetApiKeyAsync(Guid apiKeyId)
    {
        var apiKeyEntity = apiKeys.FirstOrDefault(a => a.Id == apiKeyId);

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

    public override Task<IEnumerable<ApiKeyModel>> ListApiKeysAsync(string? ownerId = null)
    {
        if (string.IsNullOrEmpty(ownerId))
        {
            return Task.FromResult(apiKeys.Select(a => new ApiKeyModel
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                CreatedDate = a.CreatedDate,
                ExpirationDate = a.ExpirationDate,
                OwnerId = a.OwnerId,
                Permissions = a.Permissions.Select(p => p.Name),
            }));
        }
        else
        {
            var filteredApiKeys = apiKeys.Where(a => a.OwnerId == ownerId);

            return Task.FromResult(filteredApiKeys.Select(a => new ApiKeyModel
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                CreatedDate = a.CreatedDate,
                ExpirationDate = a.ExpirationDate,
                OwnerId = a.OwnerId,
                Permissions = a.Permissions.Select(p => p.Name),
            }));
        }
    }

    public override Task RevokeApiKeyAsync(Guid apiKeyId)
    {
        var apiKey = apiKeys.FirstOrDefault(a => a.Id == apiKeyId)
            ?? throw new InvalidOperationException("Api key with does not exist");

        apiKeys.Remove(apiKey);

        return Task.CompletedTask;
    }
}
