// <copyright file="GitRepositoryTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Aggregates;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitRepository;

using Shouldly;

/// <summary>
/// Unit tests for the <see cref="GitRepository"/> aggregate.
/// </summary>
public class GitRepositoryTests
{
    /// <summary>
    /// Tests that applying GitRepositoryAdded event to an uninitialized aggregate initializes it correctly.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryAdded_ShouldInitializeAggregate()
    {
        // Arrange
        GitRepository aggregate = new();
        GitRepositoryAdded added = new(
            "org1-testrepo",
            "testrepo",
            "Test Description",
            "org1",
            GitRepositoryVisibility.Private,
            "main",
            "remote-123",
            "https://github.com/org1/testrepo.git");

        // Act
        ApplyResult result = aggregate.Apply(added);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.Id.ShouldBe("org1-testrepo");
        newAggregate.Name.ShouldBe("testrepo");
        newAggregate.Description.ShouldBe("Test Description");
        newAggregate.OrganizationId.ShouldBe("org1");
        newAggregate.Visibility.ShouldBe(GitRepositoryVisibility.Private);
        newAggregate.DefaultBranch.ShouldBe("main");
        newAggregate.RemoteId.ShouldBe("remote-123");
        newAggregate.Url.ShouldBe("https://github.com/org1/testrepo.git");
        newAggregate.Origin.ShouldBe(GitRepositoryOrigin.CreatedViaApplication);
        newAggregate.SyncStatus.ShouldBe(GitRepositorySyncStatus.Synced);
        newAggregate.Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that applying GitRepositoryAdded event to an already initialized aggregate returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryAdded_WhenAlreadyInitialized_ShouldReturnError()
    {
        // Arrange
        GitRepository aggregate = new(
            "existing-id",
            "ExistingRepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryAdded added = new(
            "test-id",
            "testrepo",
            null,
            "org1",
            GitRepositoryVisibility.Private,
            "main",
            null,
            null);

        // Act
        ApplyResult result = aggregate.Apply(added);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitRepositorySynced event to an uninitialized aggregate initializes it correctly.
    /// </summary>
    [Fact]
    public void Apply_GitRepositorySynced_ShouldInitializeAggregate()
    {
        // Arrange
        GitRepository aggregate = new();
        DateTimeOffset syncedAt = DateTimeOffset.UtcNow;
        GitRepositorySynced synced = new(
            "org1-testrepo",
            "testrepo",
            "Test Description",
            "org1",
            GitRepositoryVisibility.Public,
            "main",
            "remote-123",
            "https://github.com/org1/testrepo.git",
            syncedAt);

        // Act
        ApplyResult result = aggregate.Apply(synced);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.Id.ShouldBe("org1-testrepo");
        newAggregate.Name.ShouldBe("testrepo");
        newAggregate.Description.ShouldBe("Test Description");
        newAggregate.OrganizationId.ShouldBe("org1");
        newAggregate.Visibility.ShouldBe(GitRepositoryVisibility.Public);
        newAggregate.DefaultBranch.ShouldBe("main");
        newAggregate.RemoteId.ShouldBe("remote-123");
        newAggregate.Url.ShouldBe("https://github.com/org1/testrepo.git");
        newAggregate.Origin.ShouldBe(GitRepositoryOrigin.Synced);
        newAggregate.SyncStatus.ShouldBe(GitRepositorySyncStatus.Synced);
        newAggregate.LastSyncedAt.ShouldBe(syncedAt);
        newAggregate.Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that applying GitRepositorySynced event to an initialized aggregate updates it.
    /// </summary>
    [Fact]
    public void Apply_GitRepositorySynced_WhenAlreadyInitialized_ShouldUpdateAggregate()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            "Old Description",
            "https://github.com/org1/testrepo.git",
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            "old-remote",
            GitRepositorySyncStatus.NotFoundOnRemote,
            null,
            false);
        DateTimeOffset syncedAt = DateTimeOffset.UtcNow;
        GitRepositorySynced synced = new(
            "org1-testrepo",
            "testrepo",
            "New Description",
            "org1",
            GitRepositoryVisibility.Private,
            "develop",
            "new-remote",
            "https://github.com/org1/testrepo-new.git",
            syncedAt);

        // Act
        ApplyResult result = aggregate.Apply(synced);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeFalse();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.Description.ShouldBe("New Description");
        newAggregate.Visibility.ShouldBe(GitRepositoryVisibility.Private);
        newAggregate.DefaultBranch.ShouldBe("develop");
        newAggregate.RemoteId.ShouldBe("new-remote");
        newAggregate.Url.ShouldBe("https://github.com/org1/testrepo-new.git");
        newAggregate.SyncStatus.ShouldBe(GitRepositorySyncStatus.Synced);
        newAggregate.LastSyncedAt.ShouldBe(syncedAt);
    }

    /// <summary>
    /// Tests that applying GitRepositoryDescriptionChanged event updates the aggregate correctly.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryDescriptionChanged_ShouldUpdateAggregate()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            "Old Description",
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryDescriptionChanged changed = new("org1-testrepo", "New Description");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.Description.ShouldBe("New Description");
    }

    /// <summary>
    /// Tests that applying GitRepositoryDescriptionChanged with same value returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryDescriptionChanged_WhenSameValue_ShouldReturnError()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            "Same Description",
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryDescriptionChanged changed = new("org1-testrepo", "Same Description");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitRepositoryDescriptionChanged with null can clear the description.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryDescriptionChanged_WithNull_ShouldClearDescription()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            "Existing Description",
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryDescriptionChanged changed = new("org1-testrepo", null);

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeFalse();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.Description.ShouldBeNull();
    }

    /// <summary>
    /// Tests that applying GitRepositoryVisibilityChanged event updates the aggregate correctly.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryVisibilityChanged_ShouldUpdateAggregate()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Private,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryVisibilityChanged changed = new("org1-testrepo", GitRepositoryVisibility.Public);

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.Visibility.ShouldBe(GitRepositoryVisibility.Public);
    }

    /// <summary>
    /// Tests that applying GitRepositoryVisibilityChanged with same value returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryVisibilityChanged_WhenSameValue_ShouldReturnError()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryVisibilityChanged changed = new("org1-testrepo", GitRepositoryVisibility.Public);

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitRepositoryDefaultBranchChanged event updates the aggregate correctly.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryDefaultBranchChanged_ShouldUpdateAggregate()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryDefaultBranchChanged changed = new("org1-testrepo", "develop");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.DefaultBranch.ShouldBe("develop");
    }

    /// <summary>
    /// Tests that applying GitRepositoryDefaultBranchChanged with same value returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryDefaultBranchChanged_WhenSameValue_ShouldReturnError()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryDefaultBranchChanged changed = new("org1-testrepo", "main");

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitRepositoryMarkedNotFound event updates sync status.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryMarkedNotFound_ShouldUpdateSyncStatus()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryMarkedNotFound markedNotFound = new("org1-testrepo", DateTimeOffset.UtcNow);

        // Act
        ApplyResult result = aggregate.Apply(markedNotFound);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.SyncStatus.ShouldBe(GitRepositorySyncStatus.NotFoundOnRemote);
    }

    /// <summary>
    /// Tests that applying GitRepositoryMarkedNotFound when already not found returns error.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryMarkedNotFound_WhenAlreadyNotFound_ShouldReturnError()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.NotFoundOnRemote,
            null,
            false);
        GitRepositoryMarkedNotFound markedNotFound = new("org1-testrepo", DateTimeOffset.UtcNow);

        // Act
        ApplyResult result = aggregate.Apply(markedNotFound);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitRepositoryDisabled event disables the aggregate.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryDisabled_ShouldDisableAggregate()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryDisabled disabled = new("org1-testrepo");

        // Act
        ApplyResult result = aggregate.Apply(disabled);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.Disabled.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitRepositoryDisabled event when already disabled returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryDisabled_WhenAlreadyDisabled_ShouldReturnError()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            true);
        GitRepositoryDisabled disabled = new("org1-testrepo");

        // Act
        ApplyResult result = aggregate.Apply(disabled);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitRepositoryEnabled event enables the aggregate.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryEnabled_ShouldEnableAggregate()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            true);
        GitRepositoryEnabled enabled = new("org1-testrepo");

        // Act
        ApplyResult result = aggregate.Apply(enabled);

        // Assert
        result.ShouldNotBeNull();
        result.Aggregate.ShouldNotBeNull();
        GitRepository newAggregate = result.Aggregate.ShouldBeOfType<GitRepository>();
        newAggregate.Disabled.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that applying GitRepositoryEnabled event when already enabled returns an error.
    /// </summary>
    [Fact]
    public void Apply_GitRepositoryEnabled_WhenAlreadyEnabled_ShouldReturnError()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
        GitRepositoryEnabled enabled = new("org1-testrepo");

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
        GitRepository aggregate = new();
        GitRepositoryDescriptionChanged changed = new("test-id", "New Description");

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
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            true);
        GitRepositoryDescriptionChanged changed = new("org1-testrepo", "New Description");

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
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);

        // Act
        string aggregateName = aggregate.AggregateName;

        // Assert
        aggregateName.ShouldBe(GitRepositoryDomainHelper.GitRepositoryAggregateName);
    }

    /// <summary>
    /// Tests that the aggregate ID is correctly set.
    /// </summary>
    [Fact]
    public void AggregateId_ShouldReturnCorrectId()
    {
        // Arrange
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);

        // Act
        string aggregateId = aggregate.AggregateId;

        // Assert
        aggregateId.ShouldBe("org1-testrepo");
    }

    /// <summary>
    /// Tests that an uninitialized aggregate is not initialized.
    /// </summary>
    [Fact]
    public void IsInitialized_WhenUninitialized_ShouldReturnFalse()
    {
        // Arrange
        GitRepository aggregate = new();

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
        GitRepository aggregate = new(
            "org1-testrepo",
            "testrepo",
            null,
            null,
            "main",
            "org1",
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.Synced,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);

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
        string id = GitRepositoryDomainHelper.GenerateId("org1", "TestRepo");

        // Assert
        id.ShouldBe("org1-testrepo");
    }

    /// <summary>
    /// Tests that the GenerateId helper handles mixed case correctly.
    /// </summary>
    [Fact]
    public void GenerateId_WithMixedCase_ShouldReturnLowercase()
    {
        // Arrange & Act
        string id = GitRepositoryDomainHelper.GenerateId("ORG1", "TestRepo");

        // Assert
        id.ShouldBe("ORG1-testrepo");
    }
}
