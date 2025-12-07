// <copyright file="GitOrganizationAddedValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitOrganization;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitStorageAccount;

/// <summary>
/// Validator for GitStorageAccountAdded.
/// </summary>
public class GitOrganizationAddedValidator : AbstractValidator<GitOrganizationAdded>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganizationAddedValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public GitOrganizationAddedValidator(IStringLocalizer<Labels> localizer)
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