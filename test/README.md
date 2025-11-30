# Test Projects

This directory contains all test projects for the Hexalith.MyNewModule solution.

## Overview

The test projects follow these conventions:
- Unit tests using **xUnit** framework
- Assertions using **Shouldly** library
- Mocking using **Moq** library
- Code coverage with **Coverlet**

## Directory Structure

```
test/
└── Hexalith.MyNewModule.Tests/
    ├── Domains/
    │   ├── Aggregates/    # Aggregate unit tests
    │   ├── Commands/      # Command validation tests
    │   └── Events/        # Event serialization tests
    ├── Hexalith.MyNewModule.Tests.csproj
    └── README.md
```

## Test Project

### Hexalith.MyNewModule.Tests

Main test project covering:
- Domain aggregate behavior
- Command validation
- Event handling
- Serialization/deserialization
- API integration (if applicable)

## Running Tests

### From Command Line

```bash
# Run all tests
dotnet test

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"

# Run specific test class
dotnet test --filter "FullyQualifiedName~MyToDoAggregateTests"

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate coverage report
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
```

### From Visual Studio

1. Open Test Explorer (Test > Test Explorer)
2. Click "Run All Tests" or select specific tests
3. View results in Test Explorer window

### From VS Code

1. Install C# Dev Kit extension
2. Open Testing panel
3. Run tests from the UI

## Test Categories

### Domain Tests

Test aggregate behavior and domain logic:

```csharp
public class MyToDoAggregateTests
{
    [Fact]
    public void Apply_MyToDoAdded_ShouldInitializeAggregate()
    {
        // Arrange
        var aggregate = new MyToDo();
        var added = new MyToDoAdded("id", "name", "comments");

        // Act
        var result = aggregate.Apply(added);

        // Assert
        result.Succeeded.ShouldBeTrue();
    }
}
```

### Command Tests

Test command validation:

```csharp
public class AddMyToDoValidatorTests
{
    private readonly AddMyToDoValidator _validator = new();

    [Fact]
    public void Validate_EmptyId_ShouldFail()
    {
        // Arrange
        var command = new AddMyToDo("", "name", null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }
}
```

### Event Tests

Test event serialization:

```csharp
public class MyToDoEventSerializationTests
{
    [Fact]
    public void MyToDoAdded_ShouldRoundTripSerialize()
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
    }
}
```

## Test Conventions

### Naming Convention

```
{MethodUnderTest}_{Scenario}_{ExpectedBehavior}
```

Examples:
- `Apply_MyToDoAdded_ShouldInitializeAggregate`
- `Validate_EmptyId_ShouldReturnValidationError`
- `Serialize_MyToDoEvent_ShouldRoundTrip`

### Arrange-Act-Assert Pattern

```csharp
[Fact]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange - Set up test data and dependencies
    var input = new TestInput();

    // Act - Execute the method under test
    var result = subject.Method(input);

    // Assert - Verify the expected outcome
    result.ShouldBe(expectedValue);
}
```

### Using Shouldly

```csharp
// Basic assertions
result.ShouldBe(expected);
result.ShouldNotBeNull();
result.ShouldBeEmpty();

// Collection assertions
list.ShouldContain(item);
list.ShouldAllBe(x => x.IsValid);
list.Count.ShouldBe(5);

// Exception assertions
Should.Throw<ArgumentNullException>(() => method(null));
```

### Using Moq

```csharp
// Create mock
var mockService = new Mock<IMyService>();

// Setup
mockService
    .Setup(x => x.GetAsync(It.IsAny<string>()))
    .ReturnsAsync(new Result());

// Verify
mockService.Verify(x => x.SaveAsync(It.IsAny<Data>()), Times.Once);
```

## Code Coverage

### Local Coverage

```bash
# Generate coverage
dotnet test --collect:"XPlat Code Coverage"

# Install report generator
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:./coverage/**/coverage.cobertura.xml -targetdir:./coverage/report -reporttypes:Html
```

### CI/CD Coverage

Coverage is automatically collected in GitHub Actions and reported to:
- SonarCloud
- Codacy
- Coverity

## Best Practices

1. **Test one thing per test** - Single assertion focus
2. **Use meaningful names** - Describe what's being tested
3. **Keep tests fast** - Avoid I/O when possible
4. **Mock external dependencies** - Isolate unit under test
5. **Test edge cases** - Null, empty, boundary values
6. **Maintain test data** - Use builders or fixtures
7. **Clean up resources** - Implement IDisposable if needed
