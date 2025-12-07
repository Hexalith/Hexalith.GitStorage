// <copyright file="GitOrganizationValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Validators;

using System.Text.RegularExpressions;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitOrganization;

/// <summary>
/// Validator for GitOrganization.
/// </summary>
public partial class GitOrganizationValidator : AbstractValidator<GitOrganization>
{
    /// <summary>
    /// Regular expression pattern for valid organization names.
    /// Alphanumeric characters, hyphens, and underscores only.
    /// Cannot start or end with hyphen.
    /// Length: 1-39 characters.
    /// </summary>
    private const string NamePattern = "^[a-zA-Z0-9][a-zA-Z0-9_-]*[a-zA-Z0-9]$|^[a-zA-Z0-9]$";

    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganizationValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public GitOrganizationValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
        _ = RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer[Labels.NameRequired])
            .MinimumLength(1)
            .MaximumLength(39)
            .Matches(NameRegex())
            .WithMessage(localizer[Labels.NameInvalidFormat]);
        _ = RuleFor(x => x.GitStorageAccountId)
            .NotEmpty()
            .WithMessage(localizer[Labels.GitStorageAccountIdRequired]);
    }

    /// <summary>
    /// Gets the compiled regular expression for organization name validation.
    /// </summary>
    /// <returns>A compiled regex for name validation.</returns>
    [GeneratedRegex(NamePattern)]
    private static partial Regex NameRegex();
}
