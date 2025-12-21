// <copyright file="ChangeGitStorageAccountApiCredentialsValidatorTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Commands;

using System.Diagnostics.CodeAnalysis;

using FluentValidation.TestHelper;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitStorageAccount;

using Microsoft.Extensions.Localization;

using Moq;

/// <summary>
/// Unit tests for <see cref="GitStorageAccountApiCredentialsChangedValidator"/>.
/// </summary>
public class ChangeGitStorageAccountApiCredentialsValidatorTests
{
    private readonly GitStorageAccountApiCredentialsChangedValidator _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeGitStorageAccountApiCredentialsValidatorTests"/> class.
    /// </summary>
    public ChangeGitStorageAccountApiCredentialsValidatorTests()
    {
        Mock<IStringLocalizer<Localizations.GitStorageAccount>> localizerMock = new();
        localizerMock.Setup(l => l[It.IsAny<string>()]).Returns((string key) => new LocalizedString(key, key));
        _validator = new GitStorageAccountApiCredentialsChangedValidator(localizerMock.Object);
    }

    /// <summary>
    /// Tests that an empty access token fails validation.
    /// </summary>
    [Fact]
    public void AccessToken_Empty_ShouldFailValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "https://api.github.com",
            string.Empty,
            GitServerProviderType.GitHub);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AccessToken);
    }

    /// <summary>
    /// Tests that a non-empty access token passes validation.
    /// </summary>
    [Fact]
    public void AccessToken_NonEmpty_ShouldPassValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "https://api.github.com",
            "ghp_valid_token_12345",
            GitServerProviderType.GitHub);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.AccessToken);
    }

    /// <summary>
    /// Tests that an empty ID fails validation.
    /// </summary>
    [Fact]
    public void Id_Empty_ShouldFailValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            string.Empty,
            "https://api.github.com",
            "ghp_valid_token",
            GitServerProviderType.GitHub);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    /// <summary>
    /// Tests that an invalid enum value fails validation.
    /// </summary>
    [Fact]
    public void ProviderType_InvalidEnum_ShouldFailValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "https://api.github.com",
            "ghp_valid_token",
            (GitServerProviderType)999);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProviderType);
    }

    /// <summary>
    /// Tests that a valid enum value passes validation.
    /// </summary>
    /// <param name="providerType">The provider type to test.</param>
    [Theory]
    [InlineData(GitServerProviderType.GitHub)]
    [InlineData(GitServerProviderType.Forgejo)]
    [InlineData(GitServerProviderType.Gitea)]
    [InlineData(GitServerProviderType.Generic)]
    public void ProviderType_ValidEnum_ShouldPassValidation(GitServerProviderType providerType)
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "https://api.github.com",
            "ghp_valid_token",
            providerType);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ProviderType);
    }

    /// <summary>
    /// Tests that an empty URL fails validation.
    /// </summary>
    [Fact]
    public void ServerUrl_Empty_ShouldFailValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            string.Empty,
            "ghp_valid_token",
            GitServerProviderType.GitHub);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ServerUrl);
    }

    /// <summary>
    /// Tests that an HTTP (non-HTTPS) URL fails validation.
    /// </summary>
    [Fact]
    public void ServerUrl_HttpUrl_ShouldFailValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "http://api.github.com",
            "ghp_valid_token",
            GitServerProviderType.GitHub);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ServerUrl);
    }

    /// <summary>
    /// Tests that an invalid URL format fails validation.
    /// </summary>
    [Fact]
    public void ServerUrl_InvalidFormat_ShouldFailValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "not-a-valid-url",
            "ghp_valid_token",
            GitServerProviderType.GitHub);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ServerUrl);
    }

    /// <summary>
    /// Tests that a valid HTTPS URL passes validation.
    /// </summary>
    [Fact]
    public void ServerUrl_ValidHttpsUrl_ShouldPassValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            "https://api.github.com",
            "ghp_valid_token",
            GitServerProviderType.GitHub);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ServerUrl);
    }

    /// <summary>
    /// Tests that various valid HTTPS URLs pass validation.
    /// </summary>
    /// <param name="url">The URL to test.</param>
    [Theory]
    [InlineData("https://api.github.com")]
    [InlineData("https://forgejo.example.com/api/v1")]
    [InlineData("https://git.example.com:8443/api")]
    [InlineData("https://10.0.0.1/api")]
    [SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "N/A")]
    [SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "N/A")]
    public void ServerUrl_VariousValidHttpsUrls_ShouldPassValidation(string url)
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsChanged(
            "test-id",
            url,
            "valid_token",
            GitServerProviderType.GitHub);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsChanged> result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ServerUrl);
    }
}