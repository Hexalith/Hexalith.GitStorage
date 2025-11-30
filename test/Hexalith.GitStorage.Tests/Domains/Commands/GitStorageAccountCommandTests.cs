// <copyright file="GitStorageAccountCommandTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Commands;

using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Commands.GitStorageAccount;

using Shouldly;

/// <summary>
/// Unit tests for GitStorageAccount commands.
/// </summary>
public class GitStorageAccountCommandTests
{
    /// <summary>
    /// Tests that AddGitStorageAccount command is created with correct properties.
    /// </summary>
    [Fact]
    public void AddGitStorageAccount_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var command = new AddGitStorageAccount("test-id", "Test Name", "Test Comments");

        // Assert
        command.Id.ShouldBe("test-id");
        command.Name.ShouldBe("Test Name");
        command.Comments.ShouldBe("Test Comments");
        command.AggregateId.ShouldBe("test-id");
        AddGitStorageAccount.AggregateName.ShouldBe(GitStorageAccountDomainHelper.GitStorageAccountAggregateName);
    }

    /// <summary>
    /// Tests that AddGitStorageAccount command can have null comments.
    /// </summary>
    [Fact]
    public void AddGitStorageAccount_WithNullComments_ShouldAllowNull()
    {
        // Arrange & Act
        var command = new AddGitStorageAccount("test-id", "Test Name", null);

        // Assert
        command.Comments.ShouldBeNull();
    }

    /// <summary>
    /// Tests that ChangeGitStorageAccountDescription command is created with correct properties.
    /// </summary>
    [Fact]
    public void ChangeGitStorageAccountDescription_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var command = new ChangeGitStorageAccountDescription("test-id", "New Name", "New Comments");

        // Assert
        command.Id.ShouldBe("test-id");
        command.Name.ShouldBe("New Name");
        command.Comments.ShouldBe("New Comments");
        command.AggregateId.ShouldBe("test-id");
    }

    /// <summary>
    /// Tests that DisableGitStorageAccount command is created with correct properties.
    /// </summary>
    [Fact]
    public void DisableGitStorageAccount_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var command = new DisableGitStorageAccount("test-id");

        // Assert
        command.Id.ShouldBe("test-id");
        command.AggregateId.ShouldBe("test-id");
    }

    /// <summary>
    /// Tests that EnableGitStorageAccount command is created with correct properties.
    /// </summary>
    [Fact]
    public void EnableGitStorageAccount_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var command = new EnableGitStorageAccount("test-id");

        // Assert
        command.Id.ShouldBe("test-id");
        command.AggregateId.ShouldBe("test-id");
    }
}

