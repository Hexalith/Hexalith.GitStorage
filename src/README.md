# Source Code Directory

This directory contains the main source code for the Hexalith.GitStorage project - a unified service for managing Git storage providers (GitHub and Forgejo) including organizations and repositories.

## Directory Structure

```
src/
├── HexalithGitStorageApiServerApplication.cs  # API Server application definition
├── HexalithGitStorageWebAppApplication.cs     # WebAssembly client application definition
├── HexalithGitStorageWebServerApplication.cs  # Web Server application definition
│
└── libraries/                                    # NuGet package projects
    ├── Application/                              # Application layer (CQRS)
    ├── Domain/                                   # Domain layer (DDD)
    ├── Infrastructure/                           # Infrastructure layer
    └── Presentation/                             # Presentation layer (Blazor)
```

## Application Entry Points

### API Server Application

The `HexalithGitStorageApiServerApplication.cs` defines the API server configuration:

```csharp
public class HexalithGitStorageApiServerApplication : HexalithApiServerApplication
{
    public override IEnumerable<Type> ApiServerModules => [
        typeof(HexalithUIComponentsApiServerModule),
        typeof(HexalithSecurityApiServerModule),
        typeof(HexalithGitStorageApiServerModule)
    ];
    
    public override string Id => $"{HexalithGitStorageInformation.Id}.{ApplicationType}";
    public override string Name => $"{HexalithGitStorageInformation.Name} {ApplicationType}";
}
```

### Web Server Application

The `HexalithGitStorageWebServerApplication.cs` defines the server-side rendered web application:

```csharp
public class HexalithGitStorageWebServerApplication : HexalithWebServerApplication
{
    public override Type WebAppApplicationType => typeof(HexalithGitStorageWebAppApplication);
    
    public override IEnumerable<Type> WebServerModules => [
        typeof(HexalithUIComponentsWebServerModule),
        typeof(HexalithSecurityWebServerModule),
        typeof(HexalithGitStorageAccountWebServerModule)
    ];
}
```

### WebApp Application

The `HexalithGitStorageWebAppApplication.cs` defines the WebAssembly client application for interactive client-side rendering.

## Layer Dependencies

```
Presentation Layer
       ↓
Infrastructure Layer
       ↓
Application Layer
       ↓
Domain Layer
```

Dependencies flow downward only. Upper layers depend on lower layers, but never the reverse.

## Building

From this directory:

```bash
# Restore dependencies
dotnet restore

# Build all projects
dotnet build

# Build specific project
dotnet build libraries/Application/Hexalith.GitStorage/
```

## Adding New Projects

When adding new projects to the `libraries/` directory:

1. Create the project in the appropriate layer folder
2. Follow the naming convention: `Hexalith.GitStorage.{Purpose}`
3. Add project reference to the solution file
4. Update `Directory.Build.props` if needed
5. Add package reference to `Directory.Packages.props` if using new NuGet packages
