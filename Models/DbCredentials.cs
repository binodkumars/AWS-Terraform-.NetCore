using System.Text.Json.Serialization;

public class DbCredentials
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("host")]
    public string Host { get; set; }
}
