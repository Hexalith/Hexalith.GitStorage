// <copyright file="GitStorageAccountValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Validators;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitStorageAccount;

/// <summary>
/// Validator for GitStorageAccount.
/// </summary>
public class GitStorageAccountValidator : AbstractValidator<GitStorageAccount>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitStorageAccountValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public GitStorageAccountValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer[Labels.NameRequired]);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}