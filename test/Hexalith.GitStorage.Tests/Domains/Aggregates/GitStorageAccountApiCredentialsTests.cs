// <copyright file="GitStorageAccountApiCredentialsTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Aggregates;

using Hexalith.Domains.Results;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitStorageAccount;

using Shouldly;

/// <summary>
/// Unit tests for GitStorageAccount API credentials functionality.
/// </summary>
public class GitStorageAccountApiCredentialsTests
{
    /// <summary>
    /// Tests that applying GitStorageAccountApiCredentialsChanged event updates aggregate credentials.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountApiCredentialsChanged_ShouldUpdateCredentials()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, false, null, null, null);
        GitStorageAccountApiCredentialsChanged changed = new(
            "test-id",
            "https://api.github.com",
            "ghp_test_token_12345",
            GitServerProviderType.GitHub);

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeFalse();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.ServerUrl.ShouldBe("https://api.github.com");
        newAggregate.AccessToken.ShouldBe("ghp_test_token_12345");
        newAggregate.ProviderType.ShouldBe(GitServerProviderType.GitHub);
    }

    /// <summary>
    /// Tests that applying GitStorageAccountApiCredentialsChanged with same values returns error.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountApiCredentialsChanged_WhenSameValues_ShouldReturnError()
    {
        // Arrange
        GitStorageAccount aggregate = new(
            "test-id",
            "Test Name",
            null,
            false,
            "https://api.github.com",
            "ghp_test_token_12345",
            GitServerProviderType.GitHub);
        GitStorageAccountApiCredentialsChanged changed = new(
            "test-id",
            "https://api.github.com",
            "ghp_test_token_12345",
            GitServerProviderType.GitHub);

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that applying GitStorageAccountApiCredentialsCleared event clears credentials.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountApiCredentialsCleared_ShouldClearCredentials()
    {
        // Arrange
        GitStorageAccount aggregate = new(
            "test-id",
            "Test Name",
            null,
            false,
            "https://api.github.com",
            "ghp_test_token_12345",
            GitServerProviderType.GitHub);
        GitStorageAccountApiCredentialsCleared cleared = new("test-id");

        // Act
        ApplyResult result = aggregate.Apply(cleared);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeFalse();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.ServerUrl.ShouldBeNull();
        newAggregate.AccessToken.ShouldBeNull();
        newAggregate.ProviderType.ShouldBeNull();
    }

    /// <summary>
    /// Tests that applying GitStorageAccountApiCredentialsCleared when already cleared returns error.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountApiCredentialsCleared_WhenAlreadyCleared_ShouldReturnError()
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, false, null, null, null);
        GitStorageAccountApiCredentialsCleared cleared = new("test-id");

        // Act
        ApplyResult result = aggregate.Apply(cleared);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that GitStorageAccountAdded event with credentials initializes aggregate with credentials.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountAdded_WithCredentials_ShouldInitializeWithCredentials()
    {
        // Arrange
        GitStorageAccount aggregate = new();
        GitStorageAccountAdded added = new(
            "test-id",
            "Test Name",
            "Test Comments",
            "https://api.github.com",
            "ghp_test_token_12345",
            GitServerProviderType.GitHub);

        // Act
        ApplyResult result = aggregate.Apply(added);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeFalse();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.Id.ShouldBe("test-id");
        newAggregate.Name.ShouldBe("Test Name");
        newAggregate.ServerUrl.ShouldBe("https://api.github.com");
        newAggregate.AccessToken.ShouldBe("ghp_test_token_12345");
        newAggregate.ProviderType.ShouldBe(GitServerProviderType.GitHub);
    }

    /// <summary>
    /// Tests that GitStorageAccountAdded event with server URL but no provider type defaults to GitHub.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountAdded_WithServerUrlNoProviderType_ShouldDefaultToGitHub()
    {
        // Arrange
        GitStorageAccount aggregate = new();
        GitStorageAccountAdded added = new(
            "test-id",
            "Test Name",
            null,
            "https://api.github.com",
            "ghp_test_token_12345",
            null);

        // Act
        ApplyResult result = aggregate.Apply(added);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeFalse();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.ProviderType.ShouldBe(GitServerProviderType.GitHub);
    }

    /// <summary>
    /// Tests that different provider types are correctly stored.
    /// </summary>
    /// <param name="providerType">The provider type to test.</param>
    [Theory]
    [InlineData(GitServerProviderType.GitHub)]
    [InlineData(GitServerProviderType.Forgejo)]
    [InlineData(GitServerProviderType.Gitea)]
    [InlineData(GitServerProviderType.Generic)]
    public void Apply_GitStorageAccountApiCredentialsChanged_WithDifferentProviderTypes_ShouldStoreCorrectly(GitServerProviderType providerType)
    {
        // Arrange
        GitStorageAccount aggregate = new("test-id", "Test Name", null, false, null, null, null);
        GitStorageAccountApiCredentialsChanged changed = new(
            "test-id",
            "https://git.example.com/api/v1",
            "test_token_12345",
            providerType);

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeFalse();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.ProviderType.ShouldBe(providerType);
    }

    /// <summary>
    /// Tests that credentials can be updated to different values.
    /// </summary>
    [Fact]
    public void Apply_GitStorageAccountApiCredentialsChanged_UpdateExistingCredentials_ShouldSucceed()
    {
        // Arrange
        GitStorageAccount aggregate = new(
            "test-id",
            "Test Name",
            null,
            false,
            "https://api.github.com",
            "old_token",
            GitServerProviderType.GitHub);
        GitStorageAccountApiCredentialsChanged changed = new(
            "test-id",
            "https://forgejo.example.com/api/v1",
            "new_token",
            GitServerProviderType.Forgejo);

        // Act
        ApplyResult result = aggregate.Apply(changed);

        // Assert
        result.ShouldNotBeNull();
        result.Failed.ShouldBeFalse();
        GitStorageAccount newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
        newAggregate.ServerUrl.ShouldBe("https://forgejo.example.com/api/v1");
        newAggregate.AccessToken.ShouldBe("new_token");
        newAggregate.ProviderType.ShouldBe(GitServerProviderType.Forgejo);
    }
}
