# Tools

This directory contains utility scripts and tools for development and maintenance.

## Available Tools

### flush_redis.bat

Flushes all data from a local Redis instance.

**Usage:**
```batch
flush_redis.bat
```

**When to use:**
- Clear cached data during development
- Reset state between test runs
- Clean up after debugging

**Warning:** This will delete ALL data in the Redis instance!

## Adding New Tools

When adding new tools to this directory:

1. Create the script file (`.bat`, `.ps1`, or `.sh`)
2. Add documentation to this README
3. Include usage instructions in the script header

## Development Scripts

For other development scripts, see:
- `initialize.ps1` in the repository root - Initialize a new module
- `Hexalith.Builds/Tools/` - Build system tools

## PowerShell Scripts

For PowerShell scripts:
```powershell
# Set execution policy if needed
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Run a script
.\script-name.ps1
```

## Batch Scripts

For batch scripts:
```batch
REM Run from command prompt
script-name.bat

REM Or double-click in Windows Explorer
```

## Common Tasks

### Reset Local Development Environment

```batch
REM Flush Redis
tools\flush_redis.bat

REM Clean solution
dotnet clean

REM Restore and rebuild
dotnet restore
dotnet build
```

### Update Submodules

```powershell
git submodule update --remote --merge
```

### Generate Coverage Report

```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:./coverage/**/coverage.cobertura.xml -targetdir:./coverage/report -reporttypes:Html
```



