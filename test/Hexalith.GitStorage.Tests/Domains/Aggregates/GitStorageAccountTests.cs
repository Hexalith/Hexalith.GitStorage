// <copyright file="GitStorageAccountTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Aggregates;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Events.GitStorageAccount;

using Shouldly;

/// <summary>
/// Unit tests for the <see cref="GitStorageAccount"/> aggregate.
/// </summary>
public class GitStorageAccountTests
{
    /// <summary>
    /// Tests that applying GitStorageAccountAdded event to an uninitialized aggregate initializes it correctly.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountAdded_ShouldInitializeAggregate()
    {
        // Arrange
        GitStorageAccount aggregate = new();
        GitStorageAccountAdded added = new("test-id", "Test Name", "Test Comments");

        // Act
        ApplyResult result = aggregate.Apply(added);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.Id.ShouldBe("test-id");
        newAggregate.Name.ShouldBe("Test Name");
        newAggregate.Comments.ShouldBe("Test Comments");
        newAggregate.Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that applying GitStorageAccountAdded event to an already initialized aggregate returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountAdded_WhenAlreadyInitialized_ShouldReturnError()
    {
        // Arrange
        GitStorageAccount aggregate = new("existing-id", "Existing Name", null, false);
        GitStorageAccountAdded added = new("test-id", "Test Name", null);

        // Act
        ApplyResult result = aggregate.Apply(added);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitStorageAccountDescriptionChanged event updates the aggregate correctly.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountDescriptionChanged_ShouldUpdateAggregate()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Old Name", "Old Comments", false);
        GitStorageAccountDescriptionChanged changed = new("test-id", "New Name", "New Comments");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.Name.ShouldBe("New Name");
        newAggregate.Comments.ShouldBe("New Comments");
    }

    /// <summary>
    /// Tests that applying GitStorageAccountDescriptionChanged with same values returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountDescriptionChanged_WhenSameValues_ShouldReturnError()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Same Name", "Same Comments", false);
        GitStorageAccountDescriptionChanged changed = new("test-id", "Same Name", "Same Comments");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitStorageAccountDisabled event disables the aggregate.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountDisabled_ShouldDisableAggregate()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, false);
        GitStorageAccountDisabled disabled = new("test-id");

        // Act
        ApplyResult result = aggregate.Apply(disabled);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.Disabled.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitStorageAccountDisabled event when already disabled returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountDisabled_WhenAlreadyDisabled_ShouldReturnError()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, true);
        GitStorageAccountDisabled disabled = new("test-id");

        // Act
        ApplyResult result = aggregate.Apply(disabled);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitStorageAccountEnabled event enables the aggregate.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountEnabled_ShouldEnableAggregate()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, true);
        GitStorageAccountEnabled enabled = new("test-id");

        // Act
        ApplyResult result = aggregate.Apply(enabled);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that applying GitStorageAccountEnabled event when already enabled returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountEnabled_WhenAlreadyEnabled_ShouldReturnError()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, false);
        GitStorageAccountEnabled enabled = new("test-id");

        // Act
        ApplyResult result = aggregate.Apply(enabled);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying events to an uninitialized aggregate (except Added) returns not initialized error.
    /// </summary>
    [Fact]
    public void Apply_EventOnUninitializedAggregate_ShouldReturnNotInitializedError()
    {
        // Arrange
        GitStorageAccount aggregate = new();
        GitStorageAccountDescriptionChanged changed = new("test-id", "New Name", "New Comments");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying events to a disabled aggregate (except Enable/Disable) returns not enabled error.
    /// </summary>
    [Fact]
    public void Apply_EventOnDisabledAggregate_ShouldReturnNotEnabledError()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, true);
        GitStorageAccountDescriptionChanged changed = new("test-id", "New Name", "New Comments");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that the aggregate name is correctly set.
    /// </summary>
    [Fact]
    public void AggregateName_ShouldReturnCorrectName()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, false);

        // Act
        string aggregateName = aggregate.AggregateName;

        // Assert
        aggregateName.ShouldBe(GitStorageAccountDomainHelper.GitStorageAccountAggregateName);
    }

    /// <summary>
    /// Tests that the aggregate ID is correctly set.
    /// </summary>
    [Fact]
    public void AggregateId_ShouldReturnCorrectId()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, false);

        // Act
        string aggregateId = aggregate.AggregateId;

        // Assert
        aggregateId.ShouldBe("test-id");
    }

    /// <summary>
    /// Tests that an uninitialized aggregate is not initialized.
    /// </summary>
    [Fact]
    public void IsInitialized_WhenUninitialized_ShouldReturnFalse()
    {
        // Arrange
        GitStorageAccount aggregate = new();

        // Act
        bool isInitialized = (aggregate as IDomainAggregate).IsInitialized();

        // Assert
        isInitialized.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that an initialized aggregate is initialized.
    /// </summary>
    [Fact]
    public void IsInitialized_WhenInitialized_ShouldReturnTrue()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, false);

        // Act
        bool isInitialized = (aggregate as IDomainAggregate).IsInitialized();

        // Assert
        isInitialized.ShouldBeTrue();
    }
}
