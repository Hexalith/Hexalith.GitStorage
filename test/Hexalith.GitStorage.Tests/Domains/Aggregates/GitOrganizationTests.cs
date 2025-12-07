// <copyright file="GitOrganizationTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Aggregates;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitOrganization;

using Shouldly;

/// <summary>
/// Unit tests for the <see cref="GitOrganization"/> aggregate.
/// </summary>
public class GitOrganizationTests
{
    /// <summary>
    /// Tests that applying GitOrganizationAdded event to an uninitialized aggregate initializes it correctly.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationAdded_ShouldInitializeAggregate()
    {
        // Arrange
        GitOrganization aggregate = new();
        GitOrganizationAdded added = new("account1-testorg", "testorg", "Test Description", "account1");

        // Act
        ApplyResult result = aggregate.Apply(added);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitOrganization newAggregate = result.Aggregate.ShouldBeOfType<GitOrganization>();
        newAggregate.Id.ShouldBe("account1-testorg");
        newAggregate.Name.ShouldBe("testorg");
        newAggregate.Description.ShouldBe("Test Description");
        newAggregate.GitStorageAccountId.ShouldBe("account1");
        newAggregate.Origin.ShouldBe(GitOrganizationOrigin.CreatedViaApplication);
        newAggregate.Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that applying GitOrganizationAdded event to an already initialized aggregate returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationAdded_WhenAlreadyInitialized_ShouldReturnError()
    {
        // Arrange
        GitOrganization aggregate = new("existing-id", "ExistingOrg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false);
        GitOrganizationAdded added = new("test-id", "testorg", null, "account1");

        // Act
        ApplyResult result = aggregate.Apply(added);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitOrganizationSynced event to an uninitialized aggregate initializes it correctly.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationSynced_ShouldInitializeAggregate()
    {
        // Arrange
        GitOrganization aggregate = new();
        DateTimeOffset syncedAt = DateTimeOffset.UtcNow;
        GitOrganizationSynced synced = new("account1-testorg", "testorg", "Test Description", "account1", GitOrganizationVisibility.Public, "remote-123", syncedAt);

        // Act
        ApplyResult result = aggregate.Apply(synced);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitOrganization newAggregate = result.Aggregate.ShouldBeOfType<GitOrganization>();
        newAggregate.Id.ShouldBe("account1-testorg");
        newAggregate.Name.ShouldBe("testorg");
        newAggregate.Description.ShouldBe("Test Description");
        newAggregate.GitStorageAccountId.ShouldBe("account1");
        newAggregate.RemoteId.ShouldBe("remote-123");
        newAggregate.Origin.ShouldBe(GitOrganizationOrigin.Synced);
        newAggregate.SyncStatus.ShouldBe(GitOrganizationSyncStatus.Synced);
        newAggregate.LastSyncedAt.ShouldBe(syncedAt);
        newAggregate.Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that applying GitOrganizationSynced event to an initialized aggregate updates it.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationSynced_WhenAlreadyInitialized_ShouldUpdateAggregate()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", "Old Description", "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, "old-remote", GitOrganizationSyncStatus.NotFoundOnRemote, null, false);
        DateTimeOffset syncedAt = DateTimeOffset.UtcNow;
        GitOrganizationSynced synced = new("account1-testorg", "testorg", "New Description", "account1", GitOrganizationVisibility.Public, "new-remote", syncedAt);

        // Act
        ApplyResult result = aggregate.Apply(synced);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeFalse();
        GitOrganization newAggregate = result.Aggregate.ShouldBeOfType<GitOrganization>();
        newAggregate.Description.ShouldBe("New Description");
        newAggregate.RemoteId.ShouldBe("new-remote");
        newAggregate.SyncStatus.ShouldBe(GitOrganizationSyncStatus.Synced);
        newAggregate.LastSyncedAt.ShouldBe(syncedAt);
    }

    /// <summary>
    /// Tests that applying GitOrganizationDescriptionChanged event updates the aggregate correctly.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationDescriptionChanged_ShouldUpdateAggregate()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", "Old Description", "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false);
        GitOrganizationDescriptionChanged changed = new("account1-testorg", "newname", "New Description");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitOrganization newAggregate = result.Aggregate.ShouldBeOfType<GitOrganization>();
        newAggregate.Name.ShouldBe("newname");
        newAggregate.Description.ShouldBe("New Description");
    }

    /// <summary>
    /// Tests that applying GitOrganizationDescriptionChanged with same values returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationDescriptionChanged_WhenSameValues_ShouldReturnError()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", "Same Description", "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false);
        GitOrganizationDescriptionChanged changed = new("account1-testorg", "testorg", "Same Description");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitOrganizationMarkedNotFound event updates sync status.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationMarkedNotFound_ShouldUpdateSyncStatus()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false);
        GitOrganizationMarkedNotFound markedNotFound = new("account1-testorg", DateTimeOffset.UtcNow);

        // Act
        ApplyResult result = aggregate.Apply(markedNotFound);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitOrganization newAggregate = result.Aggregate.ShouldBeOfType<GitOrganization>();
        newAggregate.SyncStatus.ShouldBe(GitOrganizationSyncStatus.NotFoundOnRemote);
    }

    /// <summary>
    /// Tests that applying GitOrganizationMarkedNotFound when already not found returns error.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationMarkedNotFound_WhenAlreadyNotFound_ShouldReturnError()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.NotFoundOnRemote, null, false);
        GitOrganizationMarkedNotFound markedNotFound = new("account1-testorg", DateTimeOffset.UtcNow);

        // Act
        ApplyResult result = aggregate.Apply(markedNotFound);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitOrganizationDisabled event disables the aggregate.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationDisabled_ShouldDisableAggregate()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false);
        GitOrganizationDisabled disabled = new("account1-testorg");

        // Act
        ApplyResult result = aggregate.Apply(disabled);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitOrganization newAggregate = result.Aggregate.ShouldBeOfType<GitOrganization>();
        newAggregate.Disabled.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitOrganizationDisabled event when already disabled returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationDisabled_WhenAlreadyDisabled_ShouldReturnError()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, true);
        GitOrganizationDisabled disabled = new("account1-testorg");

        // Act
        ApplyResult result = aggregate.Apply(disabled);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitOrganizationEnabled event enables the aggregate.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationEnabled_ShouldEnableAggregate()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, true);
        GitOrganizationEnabled enabled = new("account1-testorg");

        // Act
        ApplyResult result = aggregate.Apply(enabled);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitOrganization newAggregate = result.Aggregate.ShouldBeOfType<GitOrganization>();
        newAggregate.Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that applying GitOrganizationEnabled event when already enabled returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitOrganizationEnabled_WhenAlreadyEnabled_ShouldReturnError()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false);
        GitOrganizationEnabled enabled = new("account1-testorg");

        // Act
        ApplyResult result = aggregate.Apply(enabled);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying events to an uninitialized aggregate (except Added/Synced) returns not initialized error.
    /// </summary>
    [Fact]
    public void Apply_EventOnUninitializedAggregate_ShouldReturnNotInitializedError()
    {
        // Arrange
        GitOrganization aggregate = new();
        GitOrganizationDescriptionChanged changed = new("test-id", "New Name", "New Description");

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
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, true);
        GitOrganizationDescriptionChanged changed = new("account1-testorg", "New Name", "New Description");

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
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false);

        // Act
        string aggregateName = aggregate.AggregateName;

        // Assert
        aggregateName.ShouldBe(GitOrganizationDomainHelper.GitOrganizationAggregateName);
    }

    /// <summary>
    /// Tests that the aggregate ID is correctly set.
    /// </summary>
    [Fact]
    public void AggregateId_ShouldReturnCorrectId()
    {
        // Arrange
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false);

        // Act
        string aggregateId = aggregate.AggregateId;

        // Assert
        aggregateId.ShouldBe("account1-testorg");
    }

    /// <summary>
    /// Tests that an uninitialized aggregate is not initialized.
    /// </summary>
    [Fact]
    public void IsInitialized_WhenUninitialized_ShouldReturnFalse()
    {
        // Arrange
        GitOrganization aggregate = new();

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
        GitOrganization aggregate = new("account1-testorg", "testorg", null, "account1", GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false);

        // Act
        bool isInitialized = (aggregate as IDomainAggregate).IsInitialized();

        // Assert
        isInitialized.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that the GenerateId helper creates correct composite keys.
    /// </summary>
    [Fact]
    public void GenerateId_ShouldCreateCorrectCompositeKey()
    {
        // Arrange & Act
        string id = GitOrganizationDomainHelper.GenerateId("account1", "TestOrg");

        // Assert
        id.ShouldBe("account1-testorg");
    }
}
