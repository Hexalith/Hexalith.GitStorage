# Hexalith.MyNewModule

This is the main application project that orchestrates the MyToDo domain logic, including command handling, event processing, and service integration.

## Overview

This project serves as the composition root for the application layer, bringing together:
- Command handlers
- Event handlers
- Aggregate providers
- Service registrations

## Directory Structure

```
Hexalith.MyNewModule/
├── CommandHandlers/
│   └── MyToDoCommandHandlerHelper.cs
├── EventHandlers/
│   └── MyToDoEventHandlerHelper.cs
├── Helpers/
│   ├── MyToDoHelper.cs
│   └── MyToDoModulePolicies.cs
└── Hexalith.MyNewModule.csproj
```

## Key Components

### MyToDoHelper

Main entry point for service registration:

```csharp
public static class MyToDoHelper
{
    public static IServiceCollection AddMyToDo(this IServiceCollection services)
    {
        _ = services.AddMyToDoCommandHandlers();
        _ = services.AddMyToDoAggregateProviders();
        _ = services.AddMyToDoEventValidators();
        return services;
    }
}
```

### MyToDoCommandHandlerHelper

Registers command handlers:

```csharp
public static class MyToDoCommandHandlerHelper
{
    public static IServiceCollection AddMyToDoCommandHandlers(
        this IServiceCollection services)
    {
        // Register command handlers for each command type
        services.AddScoped<ICommandHandler<AddMyToDo>, AddMyToDoHandler>();
        services.AddScoped<ICommandHandler<ChangeMyToDoDescription>, 
            ChangeMyToDoDescriptionHandler>();
        // ... more handlers
        return services;
    }
}
```

### MyToDoEventHandlerHelper

Registers event validators:

```csharp
public static class MyToDoEventHandlerHelper
{
    public static IServiceCollection AddMyToDoEventValidators(
        this IServiceCollection services)
    {
        // Register validators for domain events
        return services;
    }
}
```

### MyToDoModulePolicies

Defines authorization policies for the module:

```csharp
public static class MyToDoModulePolicies
{
    public static IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => 
        new Dictionary<string, AuthorizationPolicy>
        {
            [MyToDoPolicies.Owner] = new AuthorizationPolicyBuilder()
                .RequireRole(MyToDoRoles.Owner)
                .Build(),
            [MyToDoPolicies.Contributor] = new AuthorizationPolicyBuilder()
                .RequireRole(MyToDoRoles.Owner, MyToDoRoles.Contributor)
                .Build(),
            [MyToDoPolicies.Reader] = new AuthorizationPolicyBuilder()
                .RequireRole(MyToDoRoles.Owner, MyToDoRoles.Contributor, 
                    MyToDoRoles.Reader)
                .Build()
        };
}
```

## Dependencies

This project references all other MyNewModule projects:

```xml
<ItemGroup>
  <ProjectReference Include="..\Hexalith.MyNewModule.Abstractions" />
  <ProjectReference Include="..\Hexalith.MyNewModule.Commands" />
  <ProjectReference Include="..\Hexalith.MyNewModule.Projections" />
  <ProjectReference Include="..\Hexalith.MyNewModule.Requests" />
  <ProjectReference Include="..\..\Domain\Hexalith.MyNewModule.Aggregates" />
  <ProjectReference Include="..\..\Domain\Hexalith.MyNewModule.Events" />
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
    HexalithMyNewModuleEventsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModuleCommandsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModuleRequestsSerialization.RegisterPolymorphicMappers();

    // Add module services
    services.AddMyToDo();
    
    // Add projection handlers
    services.AddMyToDoProjectionActorFactories();
}
```

### In Web App

```csharp
public static void AddServices(IServiceCollection services)
{
    // Register serialization mappers
    HexalithMyNewModuleEventsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModuleCommandsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModuleRequestsSerialization.RegisterPolymorphicMappers();

    // Add query services
    services.AddMyToDoQueryServices();
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
