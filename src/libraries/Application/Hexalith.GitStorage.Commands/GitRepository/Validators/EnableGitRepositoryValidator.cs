// <copyright file="EnableGitRepositoryValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitRepository.Validators;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitRepository;

/// <summary>
/// Validator for <see cref="EnableGitRepository"/> command.
/// </summary>
public class EnableGitRepositoryValidator : AbstractValidator<EnableGitRepository>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnableGitRepositoryValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public EnableGitRepositoryValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}
