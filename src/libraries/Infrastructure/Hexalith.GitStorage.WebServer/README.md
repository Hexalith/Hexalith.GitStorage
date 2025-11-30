# Hexalith.GitStorage.WebServer

This project provides the server-side Blazor hosting module for the GitStorageAccount bounded context.

## Overview

The WebServer module handles:
- Server-side rendering (SSR) of Blazor pages
- Server-side prerendering for WASM components
- Authentication/authorization integration
- Server-side request handling

## Directory Structure

```
Hexalith.GitStorage.WebServer/
├── Controllers/
│   └── (server-side controllers)
├── Modules/
│   └── HexalithGitStorageAccountWebServerModule.cs
├── Properties/
│   └── launchSettings.json
└── Hexalith.GitStorage.WebServer.csproj
```

## Module Definition

### HexalithGitStorageAccountWebServerModule

The main module class implementing `IWebServerApplicationModule`:

```csharp
public sealed class HexalithGitStorageAccountWebServerModule : 
    IWebServerApplicationModule, IGitStorageAccountModule
{
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies 
        => GitStorageAccountModulePolicies.AuthorizationPolicies;
    
    public IEnumerable<string> Dependencies => [];
    public string Description => "Hexalith GitStorageAccount Web Server module";
    public string Id => "Hexalith.GitStorage.WebServer";
    public string Name => "Hexalith GitStorageAccount Web Server";
    public int OrderWeight => 0;
    public string Version => "1.0";

    public IEnumerable<Assembly> PresentationAssemblies => [
        GetType().Assembly,
        typeof(UI.Components._Imports).Assembly,
        typeof(UI.Pages._Imports).Assembly,
    ];
}
```

### Service Registration

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // Register serialization mappers
    HexalithGitStorageEventsSerialization.RegisterPolymorphicMappers();
    HexalithGitStorageCommandsSerialization.RegisterPolymorphicMappers();
    HexalithGitStorageRequestsSerialization.RegisterPolymorphicMappers();

    // Add application module
    services.TryAddSingleton<IGitStorageAccountModule, HexalithGitStorageAccountWebServerModule>();

    // Add server-side services
    services.AddGitStorageAccountProjections();
    
    // Add menu configuration
    services.AddTransient(_ => GitStorageMenu.Menu);
}
```

## Render Modes

The WebServer supports multiple render modes:

| Mode | Description | Use Case |
|------|-------------|----------|
| **Static** | No interactivity | Read-only pages |
| **InteractiveServer** | Server-side interactivity | Complex forms |
| **InteractiveAuto** | Auto-select based on load | Default choice |

### Using Render Modes

```razor
@* Static rendering *@
@rendermode @(new InteractiveServerRenderMode(prerender: true))

@* Interactive server-side *@
@rendermode InteractiveServer

@* Automatic selection *@
@rendermode InteractiveAuto
```

## Server-Side Features

### Request Handling

The server handles requests directly, bypassing HTTP:

```csharp
// Direct service injection
@inject IRequestService RequestService

var response = await RequestService.SubmitAsync(user, request, cancellationToken);
```

### Authentication Integration

```razor
@code {
    [CascadingParameter]
    public Task<AuthenticationState>? AuthenticationStateTask { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (AuthenticationStateTask is not null)
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;
        }
    }
}
```

### Session Management

Server-side sessions provide:
- User authentication state
- Partition context
- Session-specific data

## Presentation Assemblies

Referenced assemblies for Blazor routing:

```csharp
public IEnumerable<Assembly> PresentationAssemblies => [
    GetType().Assembly,
    typeof(UI.Components._Imports).Assembly,
    typeof(UI.Pages._Imports).Assembly,
];
```

## Integration with WebApp

The WebServer hosts the WebApp client:

```csharp
// In application definition
public override Type WebAppApplicationType => typeof(HexalithGitStorageWebAppApplication);
```

This enables:
- Server-side prerendering of WASM components
- Seamless client/server transition
- Shared component libraries

## Dependencies

- `Hexalith.Infrastructure.ClientAppOnServer` - Server hosting infrastructure
- `Hexalith.UI.WebServer` - Web server framework
- `Hexalith.GitStorage.Projections` - Projection services
- `Hexalith.GitStorage.UI.Pages` - Blazor pages

## Configuration

### appsettings.json

```json
{
  "Hexalith": {
    "IdentityStores": {
      "Microsoft": {
        "Id": "your-client-id",
        "Secret": "your-client-secret",
        "Tenant": "your-tenant-id"
      }
    }
  },
  "AllowedHosts": "*"
}
```

### Environment Variables

| Variable | Description |
|----------|-------------|
| `APP_API_TOKEN` | API authentication token |
| `Hexalith__IdentityStores__*` | Identity provider settings |

## Best Practices

1. **Use prerendering** - Improve initial load time
2. **Server-side validation** - Don't trust client data
3. **Minimize SignalR traffic** - Batch UI updates
4. **Handle disconnections** - Graceful reconnection
5. **Secure endpoints** - Apply authorization policies
