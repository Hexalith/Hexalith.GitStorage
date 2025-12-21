// <copyright file="AddGitRepositoryValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitRepository.Validators;

using System.Text.RegularExpressions;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitRepository;

/// <summary>
/// Validator for <see cref="AddGitRepository"/> command.
/// </summary>
public partial class AddGitRepositoryValidator : AbstractValidator<AddGitRepository>
{
    /// <summary>
    /// Regular expression pattern for valid repository names.
    /// - 1-100 characters
    /// - Alphanumeric, hyphens, underscores, and periods
    /// - Cannot start with a period
    /// - Cannot end with .git
    /// - Cannot contain consecutive periods.
    /// </summary>
    private const string NamePattern = @"^(?!\.)(?!.*\.\.)[a-zA-Z0-9._-]{1,100}(?<!\.git)$";

    /// <summary>
    /// Initializes a new instance of the <see cref="AddGitRepositoryValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public AddGitRepositoryValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
        _ = RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer[Labels.NameRequired])
            .MinimumLength(1)
            .MaximumLength(100)
            .Matches(NameRegex())
            .WithMessage(localizer[Labels.NameInvalidFormat]);
        _ = RuleFor(x => x.OrganizationId)
            .NotEmpty()
            .WithMessage(localizer[Labels.OrganizationIdRequired]);
        _ = RuleFor(x => x.Visibility)
            .IsInEnum()
            .WithMessage(localizer[Labels.VisibilityInvalid]);
    }

    /// <summary>
    /// Gets the compiled regular expression for repository name validation.
    /// </summary>
    /// <returns>A compiled regex for name validation.</returns>
    [GeneratedRegex(NamePattern)]
    private static partial Regex NameRegex();
}
