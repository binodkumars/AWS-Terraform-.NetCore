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
