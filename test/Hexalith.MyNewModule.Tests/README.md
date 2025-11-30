# Hexalith.MyNewModule.Tests

This project contains unit and integration tests for the MyToDo solution.

## Overview

This test project covers:
- Domain aggregate behavior tests
- Command validation tests
- Event serialization tests
- Request handler tests
- Integration tests

## Project Structure

```
Hexalith.MyNewModule.Tests/
├── Domains/
│   ├── Aggregates/        # Aggregate unit tests
│   │   └── MyToDoTests.cs
│   ├── Commands/          # Command validation tests
│   │   └── AddMyToDoTests.cs
│   └── Events/            # Event tests
│       └── MyToDoEventTests.cs
└── Hexalith.MyNewModule.Tests.csproj
```

## Dependencies

### Test Frameworks

| Package | Version | Purpose |
|---------|---------|---------|
| xunit | Latest | Test framework |
| xunit.runner.visualstudio | Latest | Test runner |
| Microsoft.NET.Test.Sdk | Latest | Test SDK |

### Assertion Libraries

| Package | Purpose |
|---------|---------|
| Shouldly | Fluent assertions |

### Mocking Libraries

| Package | Purpose |
|---------|---------|
| Moq | Mocking framework |

### Coverage Tools

| Package | Purpose |
|---------|---------|
| coverlet.collector | Code coverage |

## Project References

```xml
<ItemGroup>
  <ProjectReference Include="..\..\HexalithApp\src\HexalithApp.ApiServer" />
  <ProjectReference Include="..\..\HexalithApp\src\HexalithApp.WebServer" />
</ItemGroup>
```

## Test Categories

### Domain Aggregate Tests

Test the aggregate's event handling and business rules:

```csharp
namespace Hexalith.MyNewModule.Tests.Domains.Aggregates;

public class MyToDoTests
{
    [Fact]
    public void Constructor_WithValidEvent_ShouldCreateAggregate()
    {
        // Arrange
        var added = new MyToDoAdded("test-id", "Test Name", "Comments");
        
        // Act
        var aggregate = new MyToDo(added);
        
        // Assert
        aggregate.Id.ShouldBe("test-id");
        aggregate.Name.ShouldBe("Test Name");
        aggregate.Comments.ShouldBe("Comments");
        aggregate.Disabled.ShouldBeFalse();
    }

    [Fact]
    public void Apply_OnInitialized_ShouldReturnNotInitializedError()
    {
        // Arrange
        var aggregate = new MyToDo();
        var descriptionChanged = new MyToDoDescriptionChanged("id", "Name", null);
        
        // Act
        var result = aggregate.Apply(descriptionChanged);
        
        // Assert
        result.Succeeded.ShouldBeFalse();
        result.ErrorMessage.ShouldContain("not initialized");
    }

    [Fact]
    public void Apply_DisableOnDisabled_ShouldReturnError()
    {
        // Arrange
        var aggregate = new MyToDo("id", "Name", null, true);
        var disable = new MyToDoDisabled("id");
        
        // Act
        var result = aggregate.Apply(disable);
        
        // Assert
        result.Succeeded.ShouldBeFalse();
        result.ErrorMessage.ShouldContain("already disabled");
    }
}
```

### Command Tests

Test command validation:

```csharp
namespace Hexalith.MyNewModule.Tests.Domains.Commands;

public class AddMyToDoTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Arrange & Act
        var command = new AddMyToDo("id", "name", "comments");
        
        // Assert
        command.Id.ShouldBe("id");
        command.Name.ShouldBe("name");
        command.Comments.ShouldBe("comments");
    }

    [Fact]
    public void AggregateId_ShouldReturnId()
    {
        // Arrange
        var command = new AddMyToDo("test-id", "name", null);
        
        // Act & Assert
        command.AggregateId.ShouldBe("test-id");
    }

    [Fact]
    public void AggregateName_ShouldReturnConstant()
    {
        // Assert
        AddMyToDo.AggregateName.ShouldBe("MyToDo");
    }
}
```

### Event Tests

Test event serialization and properties:

```csharp
namespace Hexalith.MyNewModule.Tests.Domains.Events;

public class MyToDoEventTests
{
    [Fact]
    public void MyToDoAdded_ShouldSerializeAndDeserialize()
    {
        // Arrange
        var original = new MyToDoAdded("id", "name", "comments");
        
        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<MyToDoAdded>(json);
        
        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Id.ShouldBe(original.Id);
        deserialized.Name.ShouldBe(original.Name);
        deserialized.Comments.ShouldBe(original.Comments);
    }

    [Fact]
    public void MyToDoEvent_AggregateId_ShouldReturnId()
    {
        // Arrange
        var @event = new MyToDoDisabled("test-id");
        
        // Assert
        @event.AggregateId.ShouldBe("test-id");
    }
}
```

## Running Tests

### Command Line

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test
dotnet test --filter "FullyQualifiedName~MyToDoTests.Constructor"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Visual Studio

1. Open Test Explorer
2. Build the solution
3. Click "Run All" or select specific tests

## Test Data Builders

For complex test data, consider using builders:

```csharp
public class MyToDoBuilder
{
    private string _id = "default-id";
    private string _name = "Default Name";
    private string? _comments = null;
    private bool _disabled = false;

    public MyToDoBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public MyToDoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public MyToDoBuilder Disabled()
    {
        _disabled = true;
        return this;
    }

    public MyToDo Build()
    {
        return new MyToDo(_id, _name, _comments, _disabled);
    }
}
```

## Mocking Example

```csharp
public class MyServiceTests
{
    [Fact]
    public async Task ProcessAsync_ShouldCallRepository()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<MyToDo>>();
        mockRepo
            .Setup(r => r.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MyToDo("id", "name", null, false));
            
        var service = new MyService(mockRepo.Object);
        
        // Act
        await service.ProcessAsync("id", CancellationToken.None);
        
        // Assert
        mockRepo.Verify(
            r => r.GetAsync("id", It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}
```

## Best Practices

1. **Isolate tests** - Each test should be independent
2. **Use descriptive names** - Tests should document behavior
3. **Keep tests fast** - Mock I/O operations
4. **Test edge cases** - Null, empty, invalid inputs
5. **Maintain readability** - Use AAA pattern consistently
