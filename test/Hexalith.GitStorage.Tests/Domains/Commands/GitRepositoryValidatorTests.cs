// <copyright file="GitRepositoryValidatorTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Commands;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Commands.GitRepository;
using Hexalith.GitStorage.Commands.GitRepository.Validators;

using Microsoft.Extensions.Localization;

using Moq;

using Shouldly;

using Labels = Hexalith.GitStorage.Localizations.GitRepository;

/// <summary>
/// Unit tests for GitRepository command validators.
/// </summary>
public class GitRepositoryValidatorTests
{
    private readonly Mock<IStringLocalizer<Labels>> _localizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitRepositoryValidatorTests"/> class.
    /// </summary>
    public GitRepositoryValidatorTests()
    {
        _localizer = new Mock<IStringLocalizer<Labels>>();

        // Setup localizer to return the key as value for testing
        _localizer.Setup(l => l[It.IsAny<string>()])
            .Returns<string>(key => new LocalizedString(key, key));
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator passes for valid command.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var command = new AddGitRepository("org1-testrepo", "testrepo", "Description", "org1", GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var command = new AddGitRepository(string.Empty, "testrepo", "Description", "org1", GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator fails when Name is empty.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_EmptyName_ShouldFail()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var command = new AddGitRepository("org1-testrepo", string.Empty, "Description", "org1", GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator fails when Name exceeds max length.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_NameTooLong_ShouldFail()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var longName = new string('a', 101); // Max is 100
        var command = new AddGitRepository("org1-testrepo", longName, "Description", "org1", GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator fails when Name starts with period.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_NameStartsWithPeriod_ShouldFail()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var command = new AddGitRepository("org1-.testrepo", ".testrepo", "Description", "org1", GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator fails when Name ends with .git.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_NameEndsWithDotGit_ShouldFail()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var command = new AddGitRepository("org1-testrepo.git", "testrepo.git", "Description", "org1", GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator fails when OrganizationId is empty.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_EmptyOrganizationId_ShouldFail()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var command = new AddGitRepository("org1-testrepo", "testrepo", "Description", string.Empty, GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "OrganizationId");
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator allows single character names.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_SingleCharacterName_ShouldPass()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var command = new AddGitRepository("org1-a", "a", "Description", "org1", GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator allows names with hyphens, underscores, and periods.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_NameWithSpecialChars_ShouldPass()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var command = new AddGitRepository("org1-test-repo_name.v2", "test-repo_name.v2", "Description", "org1", GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that AddGitRepositoryValidator allows null description.
    /// </summary>
    [Fact]
    public void AddGitRepositoryValidator_NullDescription_ShouldPass()
    {
        // Arrange
        var validator = new AddGitRepositoryValidator(_localizer.Object);
        var command = new AddGitRepository("org1-testrepo", "testrepo", null, "org1", GitRepositoryVisibility.Private, "main");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that ChangeGitRepositoryDescriptionValidator passes for valid command.
    /// </summary>
    [Fact]
    public void ChangeGitRepositoryDescriptionValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new ChangeGitRepositoryDescriptionValidator(_localizer.Object);
        var command = new ChangeGitRepositoryDescription("org1-testrepo", "New Description");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that ChangeGitRepositoryDescriptionValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void ChangeGitRepositoryDescriptionValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new ChangeGitRepositoryDescriptionValidator(_localizer.Object);
        var command = new ChangeGitRepositoryDescription(string.Empty, "New Description");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that ChangeGitRepositoryDescriptionValidator allows null description.
    /// </summary>
    [Fact]
    public void ChangeGitRepositoryDescriptionValidator_NullDescription_ShouldPass()
    {
        // Arrange
        var validator = new ChangeGitRepositoryDescriptionValidator(_localizer.Object);
        var command = new ChangeGitRepositoryDescription("org1-testrepo", null);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that ChangeGitRepositoryVisibilityValidator passes for valid command.
    /// </summary>
    [Fact]
    public void ChangeGitRepositoryVisibilityValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new ChangeGitRepositoryVisibilityValidator(_localizer.Object);
        var command = new ChangeGitRepositoryVisibility("org1-testrepo", GitRepositoryVisibility.Public);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that ChangeGitRepositoryVisibilityValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void ChangeGitRepositoryVisibilityValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new ChangeGitRepositoryVisibilityValidator(_localizer.Object);
        var command = new ChangeGitRepositoryVisibility(string.Empty, GitRepositoryVisibility.Public);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that ChangeGitRepositoryVisibilityValidator validates enum values.
    /// </summary>
    /// <param name="visibility">The visibility value to test.</param>
    [Theory]
    [InlineData(GitRepositoryVisibility.Public)]
    [InlineData(GitRepositoryVisibility.Private)]
    [InlineData(GitRepositoryVisibility.Internal)]
    public void ChangeGitRepositoryVisibilityValidator_AllVisibilityValues_ShouldPass(GitRepositoryVisibility visibility)
    {
        // Arrange
        var validator = new ChangeGitRepositoryVisibilityValidator(_localizer.Object);
        var command = new ChangeGitRepositoryVisibility("org1-testrepo", visibility);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that ChangeGitRepositoryDefaultBranchValidator passes for valid command.
    /// </summary>
    [Fact]
    public void ChangeGitRepositoryDefaultBranchValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new ChangeGitRepositoryDefaultBranchValidator(_localizer.Object);
        var command = new ChangeGitRepositoryDefaultBranch("org1-testrepo", "develop");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that ChangeGitRepositoryDefaultBranchValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void ChangeGitRepositoryDefaultBranchValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new ChangeGitRepositoryDefaultBranchValidator(_localizer.Object);
        var command = new ChangeGitRepositoryDefaultBranch(string.Empty, "develop");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that ChangeGitRepositoryDefaultBranchValidator fails when DefaultBranch is empty.
    /// </summary>
    [Fact]
    public void ChangeGitRepositoryDefaultBranchValidator_EmptyDefaultBranch_ShouldFail()
    {
        // Arrange
        var validator = new ChangeGitRepositoryDefaultBranchValidator(_localizer.Object);
        var command = new ChangeGitRepositoryDefaultBranch("org1-testrepo", string.Empty);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "DefaultBranch");
    }

    /// <summary>
    /// Tests that DisableGitRepositoryValidator passes for valid command.
    /// </summary>
    [Fact]
    public void DisableGitRepositoryValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new DisableGitRepositoryValidator(_localizer.Object);
        var command = new DisableGitRepository("org1-testrepo");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that DisableGitRepositoryValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void DisableGitRepositoryValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new DisableGitRepositoryValidator(_localizer.Object);
        var command = new DisableGitRepository(string.Empty);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that EnableGitRepositoryValidator passes for valid command.
    /// </summary>
    [Fact]
    public void EnableGitRepositoryValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new EnableGitRepositoryValidator(_localizer.Object);
        var command = new EnableGitRepository("org1-testrepo");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that EnableGitRepositoryValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void EnableGitRepositoryValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new EnableGitRepositoryValidator(_localizer.Object);
        var command = new EnableGitRepository(string.Empty);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that SyncGitRepositoryValidator passes for valid command.
    /// </summary>
    [Fact]
    public void SyncGitRepositoryValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new SyncGitRepositoryValidator(_localizer.Object);
        var command = new SyncGitRepository("org1-testrepo");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that SyncGitRepositoryValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void SyncGitRepositoryValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new SyncGitRepositoryValidator(_localizer.Object);
        var command = new SyncGitRepository(string.Empty);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }
}
