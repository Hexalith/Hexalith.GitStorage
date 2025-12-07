// <copyright file="GitOrganizationValidatorTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Tests.Domains.Commands;

using Hexalith.GitStorage.Commands.GitOrganization;

using Microsoft.Extensions.Localization;

using Moq;

using Shouldly;

using AccountLabels = Hexalith.GitStorage.Localizations.GitStorageAccount;
using Labels = Hexalith.GitStorage.Localizations.GitOrganization;

/// <summary>
/// Unit tests for GitOrganization command validators.
/// </summary>
public class GitOrganizationValidatorTests
{
    private readonly Mock<IStringLocalizer<Labels>> _organizationLocalizer;
    private readonly Mock<IStringLocalizer<AccountLabels>> _accountLocalizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganizationValidatorTests"/> class.
    /// </summary>
    public GitOrganizationValidatorTests()
    {
        _organizationLocalizer = new Mock<IStringLocalizer<Labels>>();
        _accountLocalizer = new Mock<IStringLocalizer<AccountLabels>>();

        // Setup localizer to return the key as value for testing
        _organizationLocalizer.Setup(l => l[It.IsAny<string>()])
            .Returns<string>(key => new LocalizedString(key, key));
        _accountLocalizer.Setup(l => l[It.IsAny<string>()])
            .Returns<string>(key => new LocalizedString(key, key));
    }

    /// <summary>
    /// Tests that AddGitOrganizationValidator passes for valid command.
    /// </summary>
    [Fact]
    public void AddGitOrganizationValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new AddGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new AddGitOrganization("account1-testorg", "testorg", "Description", "account1");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that AddGitOrganizationValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void AddGitOrganizationValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new AddGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new AddGitOrganization(string.Empty, "testorg", "Description", "account1");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that AddGitOrganizationValidator fails when Name is empty.
    /// </summary>
    [Fact]
    public void AddGitOrganizationValidator_EmptyName_ShouldFail()
    {
        // Arrange
        var validator = new AddGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new AddGitOrganization("account1-testorg", string.Empty, "Description", "account1");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    /// <summary>
    /// Tests that AddGitOrganizationValidator fails when Name exceeds max length.
    /// </summary>
    [Fact]
    public void AddGitOrganizationValidator_NameTooLong_ShouldFail()
    {
        // Arrange
        var validator = new AddGitOrganizationValidator(_organizationLocalizer.Object);
        var longName = new string('a', 40); // Max is 39
        var command = new AddGitOrganization("account1-testorg", longName, "Description", "account1");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    /// <summary>
    /// Tests that AddGitOrganizationValidator fails when Name starts with hyphen.
    /// </summary>
    [Fact]
    public void AddGitOrganizationValidator_NameStartsWithHyphen_ShouldFail()
    {
        // Arrange
        var validator = new AddGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new AddGitOrganization("account1-testorg", "-testorg", "Description", "account1");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    /// <summary>
    /// Tests that AddGitOrganizationValidator fails when GitStorageAccountId is empty.
    /// </summary>
    [Fact]
    public void AddGitOrganizationValidator_EmptyGitStorageAccountId_ShouldFail()
    {
        // Arrange
        var validator = new AddGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new AddGitOrganization("account1-testorg", "testorg", "Description", string.Empty);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "GitStorageAccountId");
    }

    /// <summary>
    /// Tests that ChangeGitOrganizationDescriptionValidator passes for valid command.
    /// </summary>
    [Fact]
    public void ChangeGitOrganizationDescriptionValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new ChangeGitOrganizationDescriptionValidator(_organizationLocalizer.Object);
        var command = new ChangeGitOrganizationDescription("account1-testorg", "testorg", "New Description");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that ChangeGitOrganizationDescriptionValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void ChangeGitOrganizationDescriptionValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new ChangeGitOrganizationDescriptionValidator(_organizationLocalizer.Object);
        var command = new ChangeGitOrganizationDescription(string.Empty, "testorg", "New Description");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that DisableGitOrganizationValidator passes for valid command.
    /// </summary>
    [Fact]
    public void DisableGitOrganizationValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new DisableGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new DisableGitOrganization("account1-testorg");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that DisableGitOrganizationValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void DisableGitOrganizationValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new DisableGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new DisableGitOrganization(string.Empty);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that EnableGitOrganizationValidator passes for valid command.
    /// </summary>
    [Fact]
    public void EnableGitOrganizationValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new EnableGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new EnableGitOrganization("account1-testorg");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that EnableGitOrganizationValidator fails when Id is empty.
    /// </summary>
    [Fact]
    public void EnableGitOrganizationValidator_EmptyId_ShouldFail()
    {
        // Arrange
        var validator = new EnableGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new EnableGitOrganization(string.Empty);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
    }

    /// <summary>
    /// Tests that SyncGitOrganizationsValidator passes for valid command.
    /// </summary>
    [Fact]
    public void SyncGitOrganizationsValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new SyncGitOrganizationsValidator(_accountLocalizer.Object);
        var command = new SyncGitOrganizations("account1");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that SyncGitOrganizationsValidator fails when GitStorageAccountId is empty.
    /// </summary>
    [Fact]
    public void SyncGitOrganizationsValidator_EmptyGitStorageAccountId_ShouldFail()
    {
        // Arrange
        var validator = new SyncGitOrganizationsValidator(_accountLocalizer.Object);
        var command = new SyncGitOrganizations(string.Empty);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "GitStorageAccountId");
    }

    /// <summary>
    /// Tests that AddGitOrganizationValidator allows single character names.
    /// </summary>
    [Fact]
    public void AddGitOrganizationValidator_SingleCharacterName_ShouldPass()
    {
        // Arrange
        var validator = new AddGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new AddGitOrganization("account1-a", "a", "Description", "account1");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    /// <summary>
    /// Tests that AddGitOrganizationValidator allows names with hyphens and underscores.
    /// </summary>
    [Fact]
    public void AddGitOrganizationValidator_NameWithHyphensAndUnderscores_ShouldPass()
    {
        // Arrange
        var validator = new AddGitOrganizationValidator(_organizationLocalizer.Object);
        var command = new AddGitOrganization("account1-test-org_name", "test-org_name", "Description", "account1");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
