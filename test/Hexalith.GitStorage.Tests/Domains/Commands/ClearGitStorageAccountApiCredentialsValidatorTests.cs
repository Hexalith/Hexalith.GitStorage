// <copyright file="ClearGitStorageAccountApiCredentialsValidatorTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Commands;

using FluentValidation.TestHelper;

using Hexalith.GitStorage.Events.GitStorageAccount;

using Microsoft.Extensions.Localization;

using Moq;

/// <summary>
/// Unit tests for <see cref="GitStorageAccountApiCredentialsClearedValidator"/>.
/// </summary>
public class ClearGitStorageAccountApiCredentialsValidatorTests
{
    private readonly GitStorageAccountApiCredentialsClearedValidator _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClearGitStorageAccountApiCredentialsValidatorTests"/> class.
    /// </summary>
    public ClearGitStorageAccountApiCredentialsValidatorTests()
    {
        Mock<IStringLocalizer<Localizations.GitStorageAccount>> localizerMock = new();
        localizerMock.Setup(l => l[It.IsAny<string>()]).Returns((string key) => new LocalizedString(key, key));
        _validator = new GitStorageAccountApiCredentialsClearedValidator(localizerMock.Object);
    }

    /// <summary>
    /// Tests that an empty ID fails validation.
    /// </summary>
    [Fact]
    public void Id_Empty_ShouldFailValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsCleared(string.Empty);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsCleared> result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    /// <summary>
    /// Tests that a non-empty ID passes validation.
    /// </summary>
    [Fact]
    public void Id_NonEmpty_ShouldPassValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsCleared("test-id");

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsCleared> result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    /// <summary>
    /// Tests that a null ID fails validation.
    /// </summary>
    [Fact]
    public void Id_Null_ShouldFailValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsCleared(null!);

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsCleared> result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    /// <summary>
    /// Tests that a whitespace-only ID fails validation.
    /// </summary>
    [Fact]
    public void Id_WhitespaceOnly_ShouldFailValidation()
    {
        // Arrange
        var model = new GitStorageAccountApiCredentialsCleared("   ");

        // Act
        TestValidationResult<GitStorageAccountApiCredentialsCleared> result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}