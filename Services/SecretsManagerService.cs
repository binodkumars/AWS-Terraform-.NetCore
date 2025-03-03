using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;

public class SecretsManagerService
{
    private readonly IAmazonSecretsManager _secretsManager;
    private readonly string _secretName = "my-db-credentials";

    public SecretsManagerService()
    {
        _secretsManager = new AmazonSecretsManagerClient(RegionEndpoint.USEast1);
    }

    public async Task<DbCredentials> GetDbCredentialsAsync()
    {
        try
        {
            var request = new GetSecretValueRequest { SecretId = _secretName };
            var response = await _secretsManager.GetSecretValueAsync(request);

            return response.SecretString != null
                ? JsonSerializer.Deserialize<DbCredentials>(response.SecretString)
                : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving secret: {ex.Message}");
            return null;
        }
    }
}
