// <copyright file="GitStorageAccountApiCredentialsSerializationTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Events;

using System.Text.Json;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitStorageAccount;

using Shouldly;

/// <summary>
/// JSON serialization round-trip tests for Git API credentials events.
/// </summary>
public class GitStorageAccountApiCredentialsSerializationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
    };

    /// <summary>
    /// Tests that GitStorageAccountApiCredentialsChanged event serializes and deserializes correctly.
    /// </summary>
    [Fact]
    public void GitStorageAccountApiCredentialsChanged_RoundTrip_ShouldPreserveAllFields()
    {
        // Arrange
        var original = new GitStorageAccountApiCredentialsChanged(
            "test-id-123",
            "https://api.github.com",
            "ghp_test_token_abc123xyz",
            GitServerProviderType.GitHub);

        // Act
        string json = JsonSerializer.Serialize(original, JsonOptions);
        GitStorageAccountApiCredentialsChanged? deserialized = JsonSerializer.Deserialize<GitStorageAccountApiCredentialsChanged>(json, JsonOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Id.ShouldBe(original.Id);
        deserialized.ServerUrl.ShouldBe(original.ServerUrl);
        deserialized.AccessToken.ShouldBe(original.AccessToken);
        deserialized.ProviderType.ShouldBe(original.ProviderType);
    }

    /// <summary>
    /// Tests that GitStorageAccountApiCredentialsCleared event serializes and deserializes correctly.
    /// </summary>
    [Fact]
    public void GitStorageAccountApiCredentialsCleared_RoundTrip_ShouldPreserveId()
    {
        // Arrange
        var original = new GitStorageAccountApiCredentialsCleared("test-id-123");

        // Act
        string json = JsonSerializer.Serialize(original, JsonOptions);
        GitStorageAccountApiCredentialsCleared? deserialized = JsonSerializer.Deserialize<GitStorageAccountApiCredentialsCleared>(json, JsonOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Id.ShouldBe(original.Id);
    }

    /// <summary>
    /// Tests serialization with different provider types.
    /// </summary>
    /// <param name="providerType">The provider type to test.</param>
    [Theory]
    [InlineData(GitServerProviderType.GitHub)]
    [InlineData(GitServerProviderType.Forgejo)]
    [InlineData(GitServerProviderType.Gitea)]
    [InlineData(GitServerProviderType.Generic)]
    public void GitStorageAccountApiCredentialsChanged_RoundTrip_DifferentProviderTypes_ShouldPreserve(GitServerProviderType providerType)
    {
        // Arrange
        var original = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "https://git.example.com/api/v1",
            "token_12345",
            providerType);

        // Act
        string json = JsonSerializer.Serialize(original, JsonOptions);
        GitStorageAccountApiCredentialsChanged? deserialized = JsonSerializer.Deserialize<GitStorageAccountApiCredentialsChanged>(json, JsonOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.ProviderType.ShouldBe(providerType);
    }

    /// <summary>
    /// Tests that GitStorageAccountAdded with credentials serializes and deserializes correctly.
    /// </summary>
    [Fact]
    public void GitStorageAccountAdded_WithCredentials_RoundTrip_ShouldPreserveAllFields()
    {
        // Arrange
        var original = new GitStorageAccountAdded(
            "test-id-123",
            "Test Storage Account",
            "Optional comments",
            "https://api.github.com",
            "ghp_test_token",
            GitServerProviderType.Forgejo);

        // Act
        string json = JsonSerializer.Serialize(original, JsonOptions);
        GitStorageAccountAdded? deserialized = JsonSerializer.Deserialize<GitStorageAccountAdded>(json, JsonOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Id.ShouldBe(original.Id);
        deserialized.Name.ShouldBe(original.Name);
        deserialized.Comments.ShouldBe(original.Comments);
        deserialized.ServerUrl.ShouldBe(original.ServerUrl);
        deserialized.AccessToken.ShouldBe(original.AccessToken);
        deserialized.ProviderType.ShouldBe(original.ProviderType);
    }

    /// <summary>
    /// Tests that GitStorageAccountAdded without credentials serializes correctly.
    /// </summary>
    [Fact]
    public void GitStorageAccountAdded_WithoutCredentials_RoundTrip_ShouldPreserveNullFields()
    {
        // Arrange
        var original = new GitStorageAccountAdded(
            "test-id-123",
            "Test Storage Account",
            null);

        // Act
        string json = JsonSerializer.Serialize(original, JsonOptions);
        GitStorageAccountAdded? deserialized = JsonSerializer.Deserialize<GitStorageAccountAdded>(json, JsonOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.Id.ShouldBe(original.Id);
        deserialized.Name.ShouldBe(original.Name);
        deserialized.Comments.ShouldBeNull();
        deserialized.ServerUrl.ShouldBeNull();
        deserialized.AccessToken.ShouldBeNull();
        deserialized.ProviderType.ShouldBeNull();
    }

    /// <summary>
    /// Tests that special characters in access token are preserved.
    /// </summary>
    [Fact]
    public void GitStorageAccountApiCredentialsChanged_WithSpecialCharactersInToken_ShouldPreserve()
    {
        // Arrange
        var original = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "https://api.github.com",
            "token_with/special+chars=and-symbols!@#$%",
            GitServerProviderType.GitHub);

        // Act
        string json = JsonSerializer.Serialize(original, JsonOptions);
        GitStorageAccountApiCredentialsChanged? deserialized = JsonSerializer.Deserialize<GitStorageAccountApiCredentialsChanged>(json, JsonOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.AccessToken.ShouldBe(original.AccessToken);
    }

    /// <summary>
    /// Tests that URL with query parameters is preserved.
    /// </summary>
    [Fact]
    public void GitStorageAccountApiCredentialsChanged_WithUrlQueryParams_ShouldPreserve()
    {
        // Arrange
        var original = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "https://api.github.com/path?param1=value1&param2=value2",
            "token_12345",
            GitServerProviderType.GitHub);

        // Act
        string json = JsonSerializer.Serialize(original, JsonOptions);
        GitStorageAccountApiCredentialsChanged? deserialized = JsonSerializer.Deserialize<GitStorageAccountApiCredentialsChanged>(json, JsonOptions);

        // Assert
        deserialized.ShouldNotBeNull();
        deserialized.ServerUrl.ShouldBe(original.ServerUrl);
    }
}
