using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/database")]
public class DatabaseController : ControllerBase
{
    private readonly SecretsManagerService _secretsManagerService;

    public DatabaseController(SecretsManagerService secretsManagerService)
    {
        _secretsManagerService = secretsManagerService;
    }

    [HttpGet("credentials")]
    public async Task<IActionResult> GetDbCredentials()
    {
        var credentials = await _secretsManagerService.GetDbCredentialsAsync();
        if (credentials == null)
        {
            return NotFound("Failed to fetch database credentials.");
        }
        return Ok(credentials);
    }
}
