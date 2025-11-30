# Hexalith.GitStorage

This is the main application project that orchestrates the GitStorageAccount domain logic, including command handling, event processing, and service integration.

## Overview

This project serves as the composition root for the application layer, bringing together:
- Command handlers
- Event handlers
- Aggregate providers
- Service registrations

## Directory Structure

```
Hexalith.GitStorage/
├── CommandHandlers/
│   └── GitStorageAccountCommandHandlerHelper.cs
├── EventHandlers/
│   └── GitStorageAccountEventHandlerHelper.cs
├── Helpers/
│   ├── GitStorageAccountHelper.cs
│   └── GitStorageAccountModulePolicies.cs
└── Hexalith.GitStorage.csproj
```

## Key Components

### GitStorageAccountHelper

Main entry point for service registration:

```csharp
public static class GitStorageAccountHelper
{
    public static IServiceCollection AddGitStorageAccount(this IServiceCollection services)
    {
        _ = services.AddGitStorageAccountCommandHandlers();
        _ = services.AddGitStorageAccountAggregateProviders();
        _ = services.AddGitStorageAccountEventValidators();
        return services;
    }
}
```

### GitStorageAccountCommandHandlerHelper

Registers command handlers:

```csharp
public static class GitStorageAccountCommandHandlerHelper
{
    public static IServiceCollection AddGitStorageAccountCommandHandlers(
        this IServiceCollection services)
    {
        // Register command handlers for each command type
        services.AddScoped<ICommandHandler<AddGitStorageAccount>, AddGitStorageAccountHandler>();
        services.AddScoped<ICommandHandler<ChangeGitStorageAccountDescription>, 
            ChangeGitStorageAccountDescriptionHandler>();
        // ... more handlers
        return services;
    }
}
```

### GitStorageAccountEventHandlerHelper

Registers event validators:

```csharp
public static class GitStorageAccountEventHandlerHelper
{
    public static IServiceCollection AddGitStorageAccountEventValidators(
        this IServiceCollection services)
    {
        // Register validators for domain events
        return services;
    }
}
```

### GitStorageAccountModulePolicies

Defines authorization policies for the module:

```csharp
public static class GitStorageAccountModulePolicies
{
    public static IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => 
        new Dictionary<string, AuthorizationPolicy>
        {
            [GitStorageAccountPolicies.Owner] = new AuthorizationPolicyBuilder()
                .RequireRole(GitStorageAccountRoles.Owner)
                .Build(),
            [GitStorageAccountPolicies.Contributor] = new AuthorizationPolicyBuilder()
                .RequireRole(GitStorageAccountRoles.Owner, GitStorageAccountRoles.Contributor)
                .Build(),
            [GitStorageAccountPolicies.Reader] = new AuthorizationPolicyBuilder()
                .RequireRole(GitStorageAccountRoles.Owner, GitStorageAccountRoles.Contributor, 
                    GitStorageAccountRoles.Reader)
                .Build()
        };
}
```

## Dependencies

This project references all other GitStorage projects:

```xml
<ItemGroup>
  <ProjectReference Include="..\Hexalith.GitStorage.Abstractions" />
  <ProjectReference Include="..\Hexalith.GitStorage.Commands" />
  <ProjectReference Include="..\Hexalith.GitStorage.Projections" />
  <ProjectReference Include="..\Hexalith.GitStorage.Requests" />
  <ProjectReference Include="..\..\Domain\Hexalith.GitStorage.Aggregates" />
  <ProjectReference Include="..\..\Domain\Hexalith.GitStorage.Events" />
</ItemGroup>
```

External packages:
- `Hexalith.Application` - Application framework
- `Hexalith.Domains.Abstractions` - Domain abstractions
- `Hexalith.PolymorphicSerializations` - Serialization support
- `Microsoft.AspNetCore.Authorization` - Authorization

## Usage

### In API Server

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // Register serialization mappers
    HexalithGitStorageEventsSerialization.RegisterPolymorphicMappers();
    HexalithGitStorageCommandsSerialization.RegisterPolymorphicMappers();
    HexalithGitStorageRequestsSerialization.RegisterPolymorphicMappers();

    // Add module services
    services.AddGitStorageAccount();
    
    // Add projection handlers
    services.AddGitStorageAccountProjectionActorFactories();
}
```

### In Web App

```csharp
public static void AddServices(IServiceCollection services)
{
    // Register serialization mappers
    HexalithGitStorageEventsSerialization.RegisterPolymorphicMappers();
    HexalithGitStorageCommandsSerialization.RegisterPolymorphicMappers();
    HexalithGitStorageRequestsSerialization.RegisterPolymorphicMappers();

    // Add query services
    services.AddGitStorageAccountQueryServices();
}
```

## Authorization

The module implements role-based access control:

| Role | Permissions |
|------|-------------|
| Owner | Full access (CRUD + Admin) |
| Contributor | Create, Read, Update |
| Reader | Read only |

## Best Practices

1. **Register all services** - Ensure all handlers are properly registered
2. **Use dependency injection** - Follow DI patterns for testability
3. **Validate early** - Use validators in command handlers
4. **Handle exceptions** - Implement proper error handling
5. **Log operations** - Use logging for debugging and auditing
