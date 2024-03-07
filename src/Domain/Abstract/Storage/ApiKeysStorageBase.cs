namespace Dabitco.Permissioneer.Domain.Abstract.Storage;

using System.Security.Cryptography;
using System.Text;
using Dabitco.Permissioneer.Domain.Models;

public abstract class ApiKeysStorageBase
{
    public abstract Task<string> AddApiKeyAsync(ApiKeyAddRequest addRequest);
    public abstract Task<ApiKeyModel?> GetApiKeyAsync(string apiKey);
    public abstract Task<ApiKeyModel?> GetApiKeyAsync(Guid apiKeyId);
    public abstract Task<IEnumerable<ApiKeyModel>> ListApiKeysAsync(string? ownerId = null);
    public abstract Task RevokeApiKeyAsync(Guid apiKeyId);

    protected static string GenerateSecureApiKey(int byteSize = 32)
    {
        byte[] apiKeyBytes = RandomNumberGenerator.GetBytes(byteSize);

        return Convert.ToBase64String(apiKeyBytes);
    }

    protected static string HashApiKey(string apiKey)
    {
        byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(apiKey));
        StringBuilder builder = new();

        foreach (byte b in hashedBytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}
