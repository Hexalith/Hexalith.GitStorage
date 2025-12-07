// <copyright file="EnableGitOrganizationValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitOrganization;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitOrganization;

/// <summary>
/// Validator for <see cref="EnableGitOrganization"/> command.
/// </summary>
public class EnableGitOrganizationValidator : AbstractValidator<EnableGitOrganization>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnableGitOrganizationValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public EnableGitOrganizationValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}
