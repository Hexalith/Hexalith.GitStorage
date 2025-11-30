# Source Code Directory

This directory contains the main source code for the Hexalith MyToDo project.

## Directory Structure

```
src/
├── HexalithMyNewModuleApiServerApplication.cs  # API Server application definition
├── HexalithMyNewModuleWebAppApplication.cs     # WebAssembly client application definition
├── HexalithMyNewModuleWebServerApplication.cs  # Web Server application definition
│
└── libraries/                                    # NuGet package projects
    ├── Application/                              # Application layer (CQRS)
    ├── Domain/                                   # Domain layer (DDD)
    ├── Infrastructure/                           # Infrastructure layer
    └── Presentation/                             # Presentation layer (Blazor)
```

## Application Entry Points

### API Server Application

The `HexalithMyNewModuleApiServerApplication.cs` defines the API server configuration:

```csharp
public class HexalithMyNewModuleApiServerApplication : HexalithApiServerApplication
{
    public override IEnumerable<Type> ApiServerModules => [
        typeof(HexalithUIComponentsApiServerModule),
        typeof(HexalithSecurityApiServerModule),
        typeof(HexalithMyNewModuleApiServerModule)
    ];
    
    public override string Id => $"{HexalithMyNewModuleInformation.Id}.{ApplicationType}";
    public override string Name => $"{HexalithMyNewModuleInformation.Name} {ApplicationType}";
}
```

### Web Server Application

The `HexalithMyNewModuleWebServerApplication.cs` defines the server-side rendered web application:

```csharp
public class HexalithMyNewModuleWebServerApplication : HexalithWebServerApplication
{
    public override Type WebAppApplicationType => typeof(HexalithMyNewModuleWebAppApplication);
    
    public override IEnumerable<Type> WebServerModules => [
        typeof(HexalithUIComponentsWebServerModule),
        typeof(HexalithSecurityWebServerModule),
        typeof(HexalithMyToDoWebServerModule)
    ];
}
```

### WebApp Application

The `HexalithMyNewModuleWebAppApplication.cs` defines the WebAssembly client application for interactive client-side rendering.

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
dotnet build libraries/Application/Hexalith.MyNewModule/
```

## Adding New Projects

When adding new projects to the `libraries/` directory:

1. Create the project in the appropriate layer folder
2. Follow the naming convention: `Hexalith.MyNewModule.{Purpose}`
3. Add project reference to the solution file
4. Update `Directory.Build.props` if needed
5. Add package reference to `Directory.Packages.props` if using new NuGet packages
