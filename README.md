# AWS Secrets Manager with ASP.NET Core and Terraform

## Overview
This project demonstrates how to securely fetch **database credentials** from **AWS Secrets Manager** in an **ASP.NET Core Web API** using **Terraform**.

## Features
- Uses **Terraform** to store database credentials in AWS Secrets Manager.
- Fetches secrets dynamically using **AWS SDK** in an ASP.NET Core application.
- Exposes a secure API endpoint to retrieve credentials.
- Follows best security practices using IAM roles.

---

## Prerequisites
### **1. Install Terraform**
[Download and Install Terraform](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli)

### **2. Install .NET SDK**
[Download and Install .NET 6 or later](https://dotnet.microsoft.com/en-us/download)

### **3. Configure AWS CLI**
Set up AWS CLI and authenticate with IAM permissions:
```sh
aws configure
```
---

## **Step 1: Deploy AWS Secrets Manager using Terraform**

### **Initialize Terraform**
```sh
terraform init
```

### **Apply Terraform Configuration**
```sh
terraform apply -auto-approve
```
This will create a secret in AWS Secrets Manager with the following JSON structure:
```json
{
  "username": "dbuser",
  "password": "dbpassword",
  "host": "mydb.example.com"
}
```

---

## **Step 2: Run ASP.NET Core API**

### **Clone the Repository**
```sh
git clone https://github.com/yourusername/AwsSecretsManagerDemo.git
cd AwsSecretsManagerDemo
```

### **Install Dependencies**
```sh
dotnet restore
```

### **Run the Application**
```sh
dotnet run
```

---

## **Step 3: Test the API**
### **Using Postman or cURL**
```sh
curl -X GET https://localhost:5001/api/database/credentials
```
**Expected Response:**
```json
{
  "username": "dbuser",
  "password": "dbpassword",
  "host": "mydb.example.com"
}
```

---

## **Project Structure**
```
AwsSecretsManagerDemo/
│── Controllers/
│   ├── DatabaseController.cs
│── Models/
│   ├── DbCredentials.cs
│── Services/
│   ├── SecretsManagerService.cs
│── Program.cs
│── aws-secrets.tf  (Terraform Configuration)
│── AwsSecretsManagerDemo.csproj
```

---

## **Security Best Practices**
- **Use IAM Roles:** Instead of storing AWS credentials, use IAM roles for authentication.
- **Restrict Secrets Access:** Grant least privilege access to `secretsmanager:GetSecretValue`.
- **Do Not Log Secrets:** Never log sensitive credentials in production.

---

## **License**
Binod Kumar Singh

---

## Step by Step Implementation for securely fetches database credentials from AWS Secrets Manager using Terraform.

## **Step 1: Set Up AWS Secrets Manager with Terraform**
Create a Terraform file (aws-secrets.tf):

```hcl
provider "aws" {
  region = "us-east-1"
}

resource "aws_secretsmanager_secret" "db_secret" {
  name = "my-db-credentials"
}

resource "aws_secretsmanager_secret_version" "db_secret_version" {
  secret_id     = aws_secretsmanager_secret.db_secret.id
  secret_string = jsonencode({
    username = "dbuser"
    password = "dbpassword"
    host     = "mydb.example.com"
  })
}

output "secret_arn" {
  value = aws_secretsmanager_secret.db_secret.arn
}
```

**Deploy the Secret**
```sh
terraform init
terraform apply -auto-approve
```

## **Step 2: Create an ASP.NET Core Project**

Create an ASP.NET Core Web API project:

```sh
dotnet new webapi -n AwsSecretsManagerDemo
cd AwsSecretsManagerDemo
```

Install the AWS SDK package:
```sh
dotnet add package AWSSDK.SecretsManager
```

## **Step 3: Implement AWS Secrets Manager Integration**

### **A. Create a Model for DB Credentials**
Create a new class Models/DbCredentials.cs:

```csharp
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
```

### **B. Create a Service to Fetch Secrets**

Create Services/SecretsManagerService.cs:
```csharp
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
```
### **C. Register the Service in Program.cs**

Modify Program.cs:
```csharp
var builder = WebApplication.CreateBuilder(args);

// Register AWS Secrets Manager Service
builder.Services.AddSingleton<SecretsManagerService>();

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

### **D. Create an API Controller**

Create Controllers/DatabaseController.cs:

```csharp
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
```
## **Step 4: Run and Test the API**

Run the API:
```sh
dotnet run
```

Test it using Postman or cURL:
```sh
curl -X GET https://localhost:5001/api/database/credentials
```
