# AspireHost

This project contains the .NET Aspire orchestration for the GitStorage distributed application, which provides a unified service for managing Git storage providers (GitHub and Forgejo), organizations, and repositories.

## Overview

The AspireHost project provides:
- Service orchestration for development
- Environment configuration management
- Dashboard for monitoring services
- Secret management integration

## Directory Structure

```
AspireHost/
├── Components/
│   ├── Common/
│   │   ├── Development/      # Development environment configs
│   │   ├── Integration/      # Integration environment configs
│   │   ├── Production/       # Production environment configs
│   │   └── Staging/          # Staging environment configs
│   ├── GitStorageApi/
│   │   └── Development/      # API-specific dev configs
│   └── GitStorageWeb/
│       └── Development/      # Web-specific dev configs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── AspireHost.csproj
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Docker Desktop (for container support)
- Dapr CLI (optional, for local Dapr development)

### Running the Application

```bash
cd AspireHost
dotnet run
```

This starts:
1. The Aspire dashboard
2. The Web Server application
3. The API Server application
4. Any configured infrastructure services

### Accessing the Dashboard

After starting, open the Aspire dashboard URL shown in the console (typically `https://localhost:17225`).

The dashboard provides:
- Service health monitoring
- Log aggregation
- Distributed tracing
- Environment variables view

## Configuration

### Program.cs

```csharp
HexalithDistributedApplication app = new(args);

app.Builder.AddForwardedHeaders();
app.Builder.Configuration.AddUserSecrets<Program>();

// Add Web Server
if (app.IsProjectEnabled<Projects.HexalithApp_WebServer>())
{
    app.AddProject<Projects.HexalithApp_WebServer>("GitStorageweb")
        .WithEnvironmentFromConfiguration("APP_API_TOKEN")
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__Id")
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__Secret")
        // ... more configuration
}

// Add API Server
if (app.IsProjectEnabled<Projects.HexalithApp_ApiServer>())
{
    app.AddProject<Projects.HexalithApp_ApiServer>("GitStorageapi")
        .WithEnvironmentFromConfiguration("EmailServer__ApplicationSecret")
        // ... more configuration
}

await app.Builder.Build().RunAsync();
```

### Environment Variables

| Variable | Description |
|----------|-------------|
| `APP_API_TOKEN` | API authentication token |
| `Hexalith__IdentityStores__Microsoft__Id` | Microsoft OAuth client ID |
| `Hexalith__IdentityStores__Microsoft__Secret` | Microsoft OAuth client secret |
| `Hexalith__IdentityStores__Microsoft__Tenant` | Microsoft OAuth tenant ID |
| `EmailServer__ApplicationSecret` | Email service API key |
| `EmailServer__FromEmail` | Default sender email |
| `EmailServer__FromName` | Default sender name |

### User Secrets

For local development, use user secrets to store sensitive configuration:

```bash
# Initialize user secrets
dotnet user-secrets init

# Set secrets
dotnet user-secrets set "APP_API_TOKEN" "your-token"
dotnet user-secrets set "Hexalith__IdentityStores__Microsoft__Id" "your-client-id"
dotnet user-secrets set "Hexalith__IdentityStores__Microsoft__Secret" "your-secret"
```

### Component Configuration

YAML configuration files in `Components/` directory define infrastructure settings:

```yaml
# Components/Common/Development/redis.yaml
apiVersion: v1
kind: Component
metadata:
  name: statestore
spec:
  type: state.redis
  metadata:
    - name: redisHost
      value: localhost:6379
```

## Services

### GitStorageweb (Web Server)

- **Purpose**: Server-side rendered Blazor application
- **Ports**: 5001 (HTTPS)
- **Dependencies**: API Server

### GitStorageapi (API Server)

- **Purpose**: REST API and event processing
- **Ports**: 5002 (HTTPS)
- **Dependencies**: Redis, CosmosDB

## Environment Profiles

### Development

Local development with debugging enabled:
- Hot reload support
- Detailed logging
- Mock services available

### Integration

Integration testing environment:
- Connects to shared test services
- Automated test data

### Staging

Pre-production environment:
- Production-like configuration
- Reduced logging

### Production

Production environment:
- Optimized performance
- Minimal logging
- Full security

## Adding New Services

### Adding a Project

```csharp
if (app.IsProjectEnabled<Projects.NewService>())
{
    app.AddProject<Projects.NewService>("newservice")
        .WithEnvironmentFromConfiguration("CONFIG_KEY");
}
```

### Adding Infrastructure

```csharp
// Add Redis
var redis = app.Builder.AddRedis("cache");

// Add SQL Server
var sql = app.Builder.AddSqlServer("sql")
    .AddDatabase("mydb");

// Reference from project
app.AddProject<Projects.MyProject>("myproject")
    .WithReference(redis)
    .WithReference(sql);
```

## Debugging

### Attach to Running Process

1. Start AspireHost
2. In Visual Studio, Debug > Attach to Process
3. Select the process to debug

### View Logs

Logs are aggregated in the Aspire dashboard:
1. Open dashboard
2. Navigate to "Logs"
3. Filter by service or severity

### Distributed Tracing

1. Open dashboard
2. Navigate to "Traces"
3. View request flow across services

## Troubleshooting

### Port Conflicts

If ports are in use:
```bash
# Find process using port
netstat -ano | findstr :5001

# Kill process
taskkill /PID <pid> /F
```

### Docker Issues

```bash
# Restart Docker
docker system prune -f
docker-compose down
docker-compose up -d
```

### Service Not Starting

1. Check the Aspire dashboard for errors
2. Review service logs
3. Verify configuration values
4. Ensure dependencies are running

## Best Practices

1. **Use user secrets** - Never commit sensitive data
2. **Configure per environment** - Use environment-specific settings
3. **Monitor health** - Use dashboard for monitoring
4. **Enable tracing** - Track requests across services
5. **Manage dependencies** - Define service dependencies explicitly



