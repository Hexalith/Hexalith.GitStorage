// <copyright file="ChangeGitRepositoryDescriptionValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitRepository.Validators;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitRepository;

/// <summary>
/// Validator for <see cref="ChangeGitRepositoryDescription"/> command.
/// </summary>
public class ChangeGitRepositoryDescriptionValidator : AbstractValidator<ChangeGitRepositoryDescription>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeGitRepositoryDescriptionValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public ChangeGitRepositoryDescriptionValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}
