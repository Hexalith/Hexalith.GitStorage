// <copyright file="DisableGitStorageAccountValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitStorageAccount;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitStorageAccount;

/// <summary>
/// Validator for <see cref="DisableGitStorageAccount"/> command.
/// </summary>
public class DisableGitStorageAccountValidator : AbstractValidator<DisableGitStorageAccount>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisableGitStorageAccountValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public DisableGitStorageAccountValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}
