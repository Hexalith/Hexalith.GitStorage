// <copyright file="GitStorageAccountDetailsViewModelTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Requests;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Requests.GitStorageAccount;

using Shouldly;

/// <summary>
/// Unit tests for <see cref="GitStorageAccountDetailsViewModel"/>.
/// </summary>
public class GitStorageAccountDetailsViewModelTests
{
    /// <summary>
    /// Tests that MaskedAccessToken returns null when token is null.
    /// </summary>
    [Fact]
    public void MaskedAccessToken_WhenNull_ShouldReturnNull()
    {
        // Arrange
        var viewModel = new GitStorageAccountDetailsViewModel(
            "test-id",
            "Test Name",
            null,
            false,
            "https://api.github.com",
            null,
            GitServerProviderType.GitHub);

        // Act
        string? result = viewModel.MaskedAccessToken;

        // Assert
        result.ShouldBeNull();
    }

    /// <summary>
    /// Tests that MaskedAccessToken returns null when token is empty.
    /// </summary>
    [Fact]
    public void MaskedAccessToken_WhenEmpty_ShouldReturnNull()
    {
        // Arrange
        var viewModel = new GitStorageAccountDetailsViewModel(
            "test-id",
            "Test Name",
            null,
            false,
            "https://api.github.com",
            string.Empty,
            GitServerProviderType.GitHub);

        // Act
        string? result = viewModel.MaskedAccessToken;

        // Assert
        result.ShouldBeNull();
    }

    /// <summary>
    /// Tests that MaskedAccessToken returns correct masking for various token lengths.
    /// Short tokens (4 chars or less) return all asterisks.
    /// Longer tokens show first 2 and last 2 chars with asterisks in the middle.
    /// </summary>
    /// <param name="token">The access token to mask.</param>
    /// <param name="expected">The expected masked result.</param>
    [Theory]
    [InlineData("a", "*")]
    [InlineData("ab", "**")]
    [InlineData("abc", "***")]
    [InlineData("abcd", "****")]
    [InlineData("abcde", "ab*de")]
    [InlineData("abcdef", "ab**ef")]
    [InlineData("ghp_token123456789xyz", "gh*****************yz")]
    public void MaskedAccessToken_WithVariousLengths_ShouldReturnCorrectMasking(string token, string expected)
    {
        // Arrange
        var viewModel = new GitStorageAccountDetailsViewModel(
            "test-id",
            "Test Name",
            null,
            false,
            "https://api.github.com",
            token,
            GitServerProviderType.GitHub);

        // Act
        string? result = viewModel.MaskedAccessToken;

        // Assert
        result.ShouldBe(expected);
    }

    /// <summary>
    /// Tests that HasApiCredentials returns true when both ServerUrl and AccessToken are present.
    /// </summary>
    [Fact]
    public void HasApiCredentials_WhenBothUrlAndTokenPresent_ShouldReturnTrue()
    {
        // Arrange
        var viewModel = new GitStorageAccountDetailsViewModel(
            "test-id",
            "Test Name",
            null,
            false,
            "https://api.github.com",
            "ghp_token_12345",
            GitServerProviderType.GitHub);

        // Act
        bool result = viewModel.HasApiCredentials;

        // Assert
        result.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that HasApiCredentials returns false when ServerUrl is null.
    /// </summary>
    [Fact]
    public void HasApiCredentials_WhenServerUrlNull_ShouldReturnFalse()
    {
        // Arrange
        var viewModel = new GitStorageAccountDetailsViewModel(
            "test-id",
            "Test Name",
            null,
            false,
            null,
            "ghp_token_12345",
            GitServerProviderType.GitHub);

        // Act
        bool result = viewModel.HasApiCredentials;

        // Assert
        result.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that HasApiCredentials returns false when AccessToken is null.
    /// </summary>
    [Fact]
    public void HasApiCredentials_WhenAccessTokenNull_ShouldReturnFalse()
    {
        // Arrange
        var viewModel = new GitStorageAccountDetailsViewModel(
            "test-id",
            "Test Name",
            null,
            false,
            "https://api.github.com",
            null,
            GitServerProviderType.GitHub);

        // Act
        bool result = viewModel.HasApiCredentials;

        // Assert
        result.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that HasApiCredentials returns false when ServerUrl is empty.
    /// </summary>
    [Fact]
    public void HasApiCredentials_WhenServerUrlEmpty_ShouldReturnFalse()
    {
        // Arrange
        var viewModel = new GitStorageAccountDetailsViewModel(
            "test-id",
            "Test Name",
            null,
            false,
            string.Empty,
            "ghp_token_12345",
            GitServerProviderType.GitHub);

        // Act
        bool result = viewModel.HasApiCredentials;

        // Assert
        result.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that HasApiCredentials returns false when AccessToken is empty.
    /// </summary>
    [Fact]
    public void HasApiCredentials_WhenAccessTokenEmpty_ShouldReturnFalse()
    {
        // Arrange
        var viewModel = new GitStorageAccountDetailsViewModel(
            "test-id",
            "Test Name",
            null,
            false,
            "https://api.github.com",
            string.Empty,
            GitServerProviderType.GitHub);

        // Act
        bool result = viewModel.HasApiCredentials;

        // Assert
        result.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that HasApiCredentials returns false when both are missing.
    /// </summary>
    [Fact]
    public void HasApiCredentials_WhenBothMissing_ShouldReturnFalse()
    {
        // Arrange
        var viewModel = new GitStorageAccountDetailsViewModel(
            "test-id",
            "Test Name",
            null,
            false);

        // Act
        bool result = viewModel.HasApiCredentials;

        // Assert
        result.ShouldBeFalse();
    }

    /// <summary>
    /// Tests that Empty creates a view model with default values.
    /// </summary>
    [Fact]
    public void Empty_ShouldReturnViewModelWithDefaultValues()
    {
        // Act
        GitStorageAccountDetailsViewModel result = GitStorageAccountDetailsViewModel.Empty;

        // Assert
        result.Id.ShouldBeEmpty();
        result.Name.ShouldBeEmpty();
        result.Comments.ShouldBeEmpty();
        result.Disabled.ShouldBeFalse();
        result.ServerUrl.ShouldBeNull();
        result.AccessToken.ShouldBeNull();
        result.ProviderType.ShouldBeNull();
        result.HasApiCredentials.ShouldBeFalse();
        result.MaskedAccessToken.ShouldBeNull();
    }
}
