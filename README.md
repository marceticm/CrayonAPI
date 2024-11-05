# CrayonAPI

This is an application for Cloud sales, that helps customers in managing subscriptions and accounts, utilizing a SQL Server instance running on Docker.

## Prerequisites

- Docker and Docker Compose (Install Docker)
- .NET 8 SDK

1. Build the project after you clone it.

## Run SQL Server in Docker

The application requires a SQL Server instance running in a Docker container.

2. In your terminal, navigate to the solution directory containing docker-compose.yml and run: docker-compose up -d

3. You can verify that the SQL Server container is running using the command: docker ps

## Apply Migrations

4. Apply database migrations by running this in your terminal: dotnet ef database update

5. Start the application

There is a Postman collection attached with examples for every endpoint.


