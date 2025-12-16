// <copyright file="GitRepositoryEventTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Events;

using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitRepository;

using Shouldly;

/// <summary>
/// Unit tests for GitRepository events.
/// </summary>
public class GitRepositoryEventTests
{
    /// <summary>
    /// Tests that GitRepositoryAdded event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitRepositoryAdded_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var added = new GitRepositoryAdded(
            "org1-testrepo",
            "testrepo",
            "Test Description",
            "org1",
            GitRepositoryVisibility.Private,
            "main",
            "remote-123",
            "https://github.com/org1/testrepo.git");

        // Assert
        added.Id.ShouldBe("org1-testrepo");
        added.Name.ShouldBe("testrepo");
        added.Description.ShouldBe("Test Description");
        added.OrganizationId.ShouldBe("org1");
        added.Visibility.ShouldBe(GitRepositoryVisibility.Private);
        added.DefaultBranch.ShouldBe("main");
        added.RemoteId.ShouldBe("remote-123");
        added.Url.ShouldBe("https://github.com/org1/testrepo.git");
        added.AggregateId.ShouldBe("org1-testrepo");
        GitRepositoryAdded.AggregateName.ShouldBe(GitRepositoryDomainHelper.GitRepositoryAggregateName);
    }

    /// <summary>
    /// Tests that GitRepositoryAdded event can have null description.
    /// </summary>
    [Fact]
    public void GitRepositoryAdded_WithNullDescription_ShouldAllowNull()
    {
        // Arrange & Act
        var added = new GitRepositoryAdded(
            "org1-testrepo",
            "testrepo",
            null,
            "org1",
            GitRepositoryVisibility.Private,
            "main",
            null,
            null);

        // Assert
        added.Description.ShouldBeNull();
        added.RemoteId.ShouldBeNull();
        added.Url.ShouldBeNull();
    }

    /// <summary>
    /// Tests that GitRepositoryAdded event can have null default branch.
    /// </summary>
    [Fact]
    public void GitRepositoryAdded_WithNullDefaultBranch_ShouldAllowNull()
    {
        // Arrange & Act
        var added = new GitRepositoryAdded(
            "org1-testrepo",
            "testrepo",
            "Description",
            "org1",
            GitRepositoryVisibility.Private,
            null,
            null,
            null);

        // Assert
        added.DefaultBranch.ShouldBeNull();
    }

    /// <summary>
    /// Tests that GitRepositorySynced event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitRepositorySynced_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        DateTimeOffset syncedAt = DateTimeOffset.UtcNow;

        // Act
        var synced = new GitRepositorySynced(
            "org1-testrepo",
            "testrepo",
            "Test Description",
            "org1",
            GitRepositoryVisibility.Public,
            "main",
            "remote-123",
            "https://github.com/org1/testrepo.git",
            syncedAt);

        // Assert
        synced.Id.ShouldBe("org1-testrepo");
        synced.Name.ShouldBe("testrepo");
        synced.Description.ShouldBe("Test Description");
        synced.OrganizationId.ShouldBe("org1");
        synced.Visibility.ShouldBe(GitRepositoryVisibility.Public);
        synced.DefaultBranch.ShouldBe("main");
        synced.RemoteId.ShouldBe("remote-123");
        synced.Url.ShouldBe("https://github.com/org1/testrepo.git");
        synced.SyncedAt.ShouldBe(syncedAt);
        synced.AggregateId.ShouldBe("org1-testrepo");
    }

    /// <summary>
    /// Tests that GitRepositorySynced event can have null optional properties.
    /// </summary>
    [Fact]
    public void GitRepositorySynced_WithNullOptionalProperties_ShouldAllowNull()
    {
        // Arrange
        DateTimeOffset syncedAt = DateTimeOffset.UtcNow;

        // Act
        var synced = new GitRepositorySynced(
            "org1-testrepo",
            "testrepo",
            null,
            "org1",
            GitRepositoryVisibility.Private,
            null,
            null,
            null,
            syncedAt);

        // Assert
        synced.Description.ShouldBeNull();
        synced.DefaultBranch.ShouldBeNull();
        synced.RemoteId.ShouldBeNull();
        synced.Url.ShouldBeNull();
    }

    /// <summary>
    /// Tests that GitRepositoryDescriptionChanged event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitRepositoryDescriptionChanged_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var changed = new GitRepositoryDescriptionChanged("org1-testrepo", "New Description");

        // Assert
        changed.Id.ShouldBe("org1-testrepo");
        changed.Description.ShouldBe("New Description");
        changed.AggregateId.ShouldBe("org1-testrepo");
    }

    /// <summary>
    /// Tests that GitRepositoryDescriptionChanged event can have null description (to clear).
    /// </summary>
    [Fact]
    public void GitRepositoryDescriptionChanged_WithNullDescription_ShouldAllowNull()
    {
        // Arrange & Act
        var changed = new GitRepositoryDescriptionChanged("org1-testrepo", null);

        // Assert
        changed.Description.ShouldBeNull();
    }

    /// <summary>
    /// Tests that GitRepositoryVisibilityChanged event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitRepositoryVisibilityChanged_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var changed = new GitRepositoryVisibilityChanged("org1-testrepo", GitRepositoryVisibility.Public);

        // Assert
        changed.Id.ShouldBe("org1-testrepo");
        changed.Visibility.ShouldBe(GitRepositoryVisibility.Public);
        changed.AggregateId.ShouldBe("org1-testrepo");
    }

    /// <summary>
    /// Tests that GitRepositoryVisibilityChanged event supports all visibility values.
    /// </summary>
    /// <param name="visibility">The visibility value to test.</param>
    [Theory]
    [InlineData(GitRepositoryVisibility.Public)]
    [InlineData(GitRepositoryVisibility.Private)]
    [InlineData(GitRepositoryVisibility.Internal)]
    public void GitRepositoryVisibilityChanged_AllVisibilityValues_ShouldBeValid(GitRepositoryVisibility visibility)
    {
        // Arrange & Act
        var changed = new GitRepositoryVisibilityChanged("org1-testrepo", visibility);

        // Assert
        changed.Visibility.ShouldBe(visibility);
    }

    /// <summary>
    /// Tests that GitRepositoryDefaultBranchChanged event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitRepositoryDefaultBranchChanged_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var changed = new GitRepositoryDefaultBranchChanged("org1-testrepo", "develop");

        // Assert
        changed.Id.ShouldBe("org1-testrepo");
        changed.DefaultBranch.ShouldBe("develop");
        changed.AggregateId.ShouldBe("org1-testrepo");
    }

    /// <summary>
    /// Tests that GitRepositoryDefaultBranchChanged event supports various branch names.
    /// </summary>
    /// <param name="branchName">The branch name to test.</param>
    [Theory]
    [InlineData("main")]
    [InlineData("master")]
    [InlineData("develop")]
    [InlineData("feature/new-feature")]
    [InlineData("release/v1.0")]
    public void GitRepositoryDefaultBranchChanged_VariousBranchNames_ShouldBeValid(string branchName)
    {
        // Arrange & Act
        var changed = new GitRepositoryDefaultBranchChanged("org1-testrepo", branchName);

        // Assert
        changed.DefaultBranch.ShouldBe(branchName);
    }

    /// <summary>
    /// Tests that GitRepositoryDisabled event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitRepositoryDisabled_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var disabled = new GitRepositoryDisabled("org1-testrepo");

        // Assert
        disabled.Id.ShouldBe("org1-testrepo");
        disabled.AggregateId.ShouldBe("org1-testrepo");
    }

    /// <summary>
    /// Tests that GitRepositoryEnabled event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitRepositoryEnabled_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var enabled = new GitRepositoryEnabled("org1-testrepo");

        // Assert
        enabled.Id.ShouldBe("org1-testrepo");
        enabled.AggregateId.ShouldBe("org1-testrepo");
    }

    /// <summary>
    /// Tests that GitRepositoryMarkedNotFound event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitRepositoryMarkedNotFound_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        DateTimeOffset markedAt = DateTimeOffset.UtcNow;

        // Act
        var markedNotFound = new GitRepositoryMarkedNotFound("org1-testrepo", markedAt);

        // Assert
        markedNotFound.Id.ShouldBe("org1-testrepo");
        markedNotFound.MarkedAt.ShouldBe(markedAt);
        markedNotFound.AggregateId.ShouldBe("org1-testrepo");
    }
}
