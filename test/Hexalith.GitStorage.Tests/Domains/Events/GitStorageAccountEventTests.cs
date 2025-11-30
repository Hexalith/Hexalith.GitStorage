// <copyright file="GitStorageAccountEventTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Events;

using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Events.GitStorageAccount;

using Shouldly;

/// <summary>
/// Unit tests for GitStorageAccount events.
/// </summary>
public class GitStorageAccountEventTests
{
    /// <summary>
    /// Tests that GitStorageAccountAdded event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitStorageAccountAdded_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var added = new GitStorageAccountAdded("test-id", "Test Name", "Test Comments");

        // Assert
        added.Id.ShouldBe("test-id");
        added.Name.ShouldBe("Test Name");
        added.Comments.ShouldBe("Test Comments");
        added.AggregateId.ShouldBe("test-id");
        GitStorageAccountAdded.AggregateName.ShouldBe(GitStorageAccountDomainHelper.GitStorageAccountAggregateName);
    }

    /// <summary>
    /// Tests that GitStorageAccountAdded event can have null comments.
    /// </summary>
    [Fact]
    public void GitStorageAccountAdded_WithNullComments_ShouldAllowNull()
    {
        // Arrange & Act
        var added = new GitStorageAccountAdded("test-id", "Test Name", null);

        // Assert
        added.Comments.ShouldBeNull();
    }

    /// <summary>
    /// Tests that GitStorageAccountDescriptionChanged event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitStorageAccountDescriptionChanged_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var changed = new GitStorageAccountDescriptionChanged("test-id", "New Name", "New Comments");

        // Assert
        changed.Id.ShouldBe("test-id");
        changed.Name.ShouldBe("New Name");
        changed.Comments.ShouldBe("New Comments");
        changed.AggregateId.ShouldBe("test-id");
    }

    /// <summary>
    /// Tests that GitStorageAccountDisabled event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitStorageAccountDisabled_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var disabled = new GitStorageAccountDisabled("test-id");

        // Assert
        disabled.Id.ShouldBe("test-id");
        disabled.AggregateId.ShouldBe("test-id");
    }

    /// <summary>
    /// Tests that GitStorageAccountEnabled event is created with correct properties.
    /// </summary>
    [Fact]
    public void GitStorageAccountEnabled_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var enabled = new GitStorageAccountEnabled("test-id");

        // Assert
        enabled.Id.ShouldBe("test-id");
        enabled.AggregateId.ShouldBe("test-id");
    }
}

