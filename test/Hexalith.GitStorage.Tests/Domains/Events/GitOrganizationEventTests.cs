// <copyright file="GitOrganizationEventTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Events;

using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Events.GitOrganization;

using Shouldly;

/// <summary>
/// Unit tests for GitOrganization events.
/// </summary>
public class GitOrganizationEventTests
{
    /// <summary>
    /// Tests that GitOrganizationAdded event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitOrganizationAdded_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var added = new GitOrganizationAdded("account1-testorg", "testorg", "Test Description", "account1");

        // Assert
        added.Id.ShouldBe("account1-testorg");
        added.Name.ShouldBe("testorg");
        added.Description.ShouldBe("Test Description");
        added.GitStorageAccountId.ShouldBe("account1");
        added.AggregateId.ShouldBe("account1-testorg");
        GitOrganizationAdded.AggregateName.ShouldBe(GitOrganizationDomainHelper.GitOrganizationAggregateName);
    }

    /// <summary>
    /// Tests that GitOrganizationAdded event can have null description.
    /// </summary>
    [Fact]
    public void GitOrganizationAdded_WithNullDescription_ShouldAllowNull()
    {
        // Arrange & Act
        var added = new GitOrganizationAdded("account1-testorg", "testorg", null, "account1");

        // Assert
        added.Description.ShouldBeNull();
    }

    /// <summary>
    /// Tests that GitOrganizationSynced event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitOrganizationSynced_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        DateTimeOffset syncedAt = DateTimeOffset.UtcNow;

        // Act
        var synced = new GitOrganizationSynced("account1-testorg", "testorg", "Test Description", "account1", "remote-123", syncedAt);

        // Assert
        synced.Id.ShouldBe("account1-testorg");
        synced.Name.ShouldBe("testorg");
        synced.Description.ShouldBe("Test Description");
        synced.GitStorageAccountId.ShouldBe("account1");
        synced.RemoteId.ShouldBe("remote-123");
        synced.SyncedAt.ShouldBe(syncedAt);
        synced.AggregateId.ShouldBe("account1-testorg");
    }

    /// <summary>
    /// Tests that GitOrganizationDescriptionChanged event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitOrganizationDescriptionChanged_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var changed = new GitOrganizationDescriptionChanged("account1-testorg", "newname", "New Description");

        // Assert
        changed.Id.ShouldBe("account1-testorg");
        changed.Name.ShouldBe("newname");
        changed.Description.ShouldBe("New Description");
        changed.AggregateId.ShouldBe("account1-testorg");
    }

    /// <summary>
    /// Tests that GitOrganizationMarkedNotFound event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitOrganizationMarkedNotFound_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        DateTimeOffset markedAt = DateTimeOffset.UtcNow;

        // Act
        var markedNotFound = new GitOrganizationMarkedNotFound("account1-testorg", markedAt);

        // Assert
        markedNotFound.Id.ShouldBe("account1-testorg");
        markedNotFound.MarkedAt.ShouldBe(markedAt);
        markedNotFound.AggregateId.ShouldBe("account1-testorg");
    }

    /// <summary>
    /// Tests that GitOrganizationDisabled event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitOrganizationDisabled_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var disabled = new GitOrganizationDisabled("account1-testorg");

        // Assert
        disabled.Id.ShouldBe("account1-testorg");
        disabled.AggregateId.ShouldBe("account1-testorg");
    }

    /// <summary>
    /// Tests that GitOrganizationEnabled event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitOrganizationEnabled_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var enabled = new GitOrganizationEnabled("account1-testorg");

        // Assert
        enabled.Id.ShouldBe("account1-testorg");
        enabled.AggregateId.ShouldBe("account1-testorg");
    }
}
