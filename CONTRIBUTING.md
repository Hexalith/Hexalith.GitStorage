# Contributing to Hexalith.GitStorage

Thank you for considering contributing to Hexalith.GitStorage! This module provides a unified API for managing Git storage providers (GitHub and Forgejo), enabling organizations and repositories to be created and managed. This document provides guidelines and instructions for contributing.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [Coding Standards](#coding-standards)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)
- [Testing Guidelines](#testing-guidelines)
- [Documentation](#documentation)

## Code of Conduct

This project adheres to a code of conduct. By participating, you are expected to:
- Be respectful and inclusive
- Accept constructive criticism gracefully
- Focus on what's best for the community
- Show empathy towards others

## Getting Started

### Prerequisites

Before contributing, ensure you have:
- [.NET 9 SDK](https://dotnet.microsoft.com/download) or later
- [PowerShell 7](https://github.com/PowerShell/PowerShell) or later
- [Git](https://git-scm.com/)
- Your preferred IDE (Visual Studio, VS Code, Cursor, Rider)

### Fork and Clone

1. Fork the repository on GitHub
2. Clone your fork:
   ```bash
   git clone https://github.com/YOUR-USERNAME/Hexalith.GitStorage.git
   cd Hexalith.GitStorage
   ```
3. Add upstream remote:
   ```bash
   git remote add upstream https://github.com/Hexalith/Hexalith.GitStorage.git
   ```
4. Initialize submodules:
   ```bash
   git submodule init
   git submodule update
   ```

## Development Setup

### 1. Restore Dependencies

```bash
dotnet restore
```

### 2. Build the Solution

```bash
dotnet build
```

### 3. Run Tests

```bash
dotnet test
```

### 4. Run the Application

```bash
cd AspireHost
dotnet run
```

## Coding Standards

### C# Guidelines

- Use **primary constructors** for classes and records when possible
- Do not duplicate properties defined in primary constructors
- Use the **latest C# language features**
- Follow the existing code style in the repository

### Documentation

- Use **XML documentation tags** for public, protected, and internal members
- Document record properties in the record comment with `<param>` tags
- Keep comments clear and concise

### Example

```csharp
/// <summary>
/// Represents a Git storage account configuration.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Name">The display name.</param>
/// <param name="ProviderType">The Git provider type (GitHub or Forgejo).</param>
/// <param name="BaseUrl">The base URL for the provider API.</param>
public record GitStorageAccountConfiguration(
    string Id,
    string Name,
    GitProviderType ProviderType,
    string BaseUrl)
{
    /// <summary>
    /// Gets an empty configuration.
    /// </summary>
    public static GitStorageAccountConfiguration Empty => new(string.Empty, string.Empty, GitProviderType.GitHub, string.Empty);
}
```

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes/Records | PascalCase | `GitStorageAccount` |
| Interfaces | IPascalCase | `IGitStorageAccountService` |
| Methods | PascalCase | `GetDetailsAsync` |
| Parameters | camelCase | `moduleName` |
| Private fields | _camelCase | `_repository` |
| Constants | PascalCase | `DefaultTimeout` |

### File Organization

- One type per file
- File name matches type name
- Organize by feature, not type

## Commit Guidelines

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

| Type | Description |
|------|-------------|
| `feat` | New feature |
| `fix` | Bug fix |
| `docs` | Documentation only |
| `style` | Code style (formatting, missing semi-colons) |
| `refactor` | Code refactoring |
| `test` | Adding or updating tests |
| `chore` | Maintenance tasks |

### Examples

```
feat(domain): add GitStorageAccount aggregate

Add the GitStorageAccount aggregate with support for:
- Add, update, and delete operations
- Event sourcing with GitStorageAccountAdded event
- Validation for all properties

Closes #123
```

```
fix(api): handle null response in GetDetails

Fixed a NullReferenceException when the module doesn't exist.
Returns NotFound result instead of throwing.

Fixes #456
```

## Pull Request Process

### Before Submitting

1. **Sync with upstream**:
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. **Create a feature branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make your changes** following coding standards

4. **Run tests**:
   ```bash
   dotnet test
   ```

5. **Build successfully**:
   ```bash
   dotnet build
   ```

### Submitting

1. Push your branch:
   ```bash
   git push origin feature/your-feature-name
   ```

2. Open a Pull Request on GitHub

3. Fill in the PR template with:
   - Description of changes
   - Related issue numbers
   - Testing performed
   - Screenshots (if UI changes)

### PR Review Checklist

- [ ] Code follows project conventions
- [ ] Tests are included and passing
- [ ] Documentation is updated
- [ ] No breaking changes (or documented)
- [ ] XML documentation for public APIs
- [ ] No merge conflicts

### After Review

- Address reviewer feedback
- Push additional commits if needed
- Once approved, the PR will be merged

## Testing Guidelines

### Test Framework

- **xUnit** for unit testing
- **Shouldly** for assertions
- **Moq** for mocking

### Test Naming

```
{MethodUnderTest}_{Scenario}_{ExpectedResult}
```

### Test Structure

```csharp
[Fact]
public void Apply_GitStorageAccountAdded_ShouldInitializeAggregate()
{
    // Arrange
    var aggregate = new GitStorageAccount();
    var added = new GitStorageAccountAdded(
        "github-main",
        "GitHub Main",
        GitProviderType.GitHub,
        "https://api.github.com",
        "Primary GitHub account");

    // Act
    var result = aggregate.Apply(added);

    // Assert
    result.Succeeded.ShouldBeTrue();
    var newAggregate = result.Aggregate as GitStorageAccount;
    newAggregate.ShouldNotBeNull();
    newAggregate.ProviderType.ShouldBe(GitProviderType.GitHub);
}
```

### Coverage Requirements

- Aim for > 80% code coverage
- Focus on business logic
- Test edge cases

## Documentation

### README Files

- Each project should have a README.md
- Document purpose, usage, and dependencies
- Include code examples

### XML Documentation

Required for:
- All public classes and interfaces
- All public methods and properties
- All protected members

### Code Comments

- Explain "why", not "what"
- Keep comments up to date
- Remove commented-out code

## Need Help?

- **Discord**: [Join our community](https://discordapp.com/channels/1102166958918610994/1102166958918610997)
- **Issues**: [GitHub Issues](https://github.com/Hexalith/Hexalith.GitStorage/issues)
- **Discussions**: [GitHub Discussions](https://github.com/Hexalith/Hexalith.GitStorage/discussions)

Thank you for contributing! 🎉


