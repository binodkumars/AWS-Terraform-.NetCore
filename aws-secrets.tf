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
