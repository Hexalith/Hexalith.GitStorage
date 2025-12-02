// <copyright file="GitOrganizationCommandTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Commands;

using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Commands.GitOrganization;

using Shouldly;

/// <summary>
/// Unit tests for GitOrganization commands.
/// </summary>
public class GitOrganizationCommandTests
{
    /// <summary>
    /// Tests that AddGitOrganization command is created with correct properties.
    /// </summary>
    [Fact]
    public void AddGitOrganization_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var command = new AddGitOrganization("account1-testorg", "testorg", "Test Description", "account1");

        // Assert
        command.Id.ShouldBe("account1-testorg");
        command.Name.ShouldBe("testorg");
        command.Description.ShouldBe("Test Description");
        command.GitStorageAccountId.ShouldBe("account1");
        command.AggregateId.ShouldBe("account1-testorg");
        AddGitOrganization.AggregateName.ShouldBe(GitOrganizationDomainHelper.GitOrganizationAggregateName);
    }

    /// <summary>
    /// Tests that AddGitOrganization command can have null description.
    /// </summary>
    [Fact]
    public void AddGitOrganization_WithNullDescription_ShouldAllowNull()
    {
        // Arrange & Act
        var command = new AddGitOrganization("account1-testorg", "testorg", null, "account1");

        // Assert
        command.Description.ShouldBeNull();
    }

    /// <summary>
    /// Tests that ChangeGitOrganizationDescription command is created with correct properties.
    /// </summary>
    [Fact]
    public void ChangeGitOrganizationDescription_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var command = new ChangeGitOrganizationDescription("account1-testorg", "newname", "New Description");

        // Assert
        command.Id.ShouldBe("account1-testorg");
        command.Name.ShouldBe("newname");
        command.Description.ShouldBe("New Description");
        command.AggregateId.ShouldBe("account1-testorg");
    }

    /// <summary>
    /// Tests that DisableGitOrganization command is created with correct properties.
    /// </summary>
    [Fact]
    public void DisableGitOrganization_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var command = new DisableGitOrganization("account1-testorg");

        // Assert
        command.Id.ShouldBe("account1-testorg");
        command.AggregateId.ShouldBe("account1-testorg");
    }

    /// <summary>
    /// Tests that EnableGitOrganization command is created with correct properties.
    /// </summary>
    [Fact]
    public void EnableGitOrganization_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var command = new EnableGitOrganization("account1-testorg");

        // Assert
        command.Id.ShouldBe("account1-testorg");
        command.AggregateId.ShouldBe("account1-testorg");
    }

    /// <summary>
    /// Tests that SyncGitOrganizations command is created with correct properties.
    /// </summary>
    [Fact]
    public void SyncGitOrganizations_Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var command = new SyncGitOrganizations("account1");

        // Assert
        command.GitStorageAccountId.ShouldBe("account1");
        command.AggregateId.ShouldBe("account1");

        // SyncGitOrganizations operates on a GitStorageAccount aggregate (syncs all orgs for that account)
        SyncGitOrganizations.AggregateName.ShouldBe(GitStorageAccountDomainHelper.GitStorageAccountAggregateName);
    }
}
