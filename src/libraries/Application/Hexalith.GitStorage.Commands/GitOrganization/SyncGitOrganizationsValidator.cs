// <copyright file="SyncGitOrganizationsValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitOrganization;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitStorageAccount;

/// <summary>
/// Validator for <see cref="SyncGitOrganizations"/> command.
/// </summary>
public class SyncGitOrganizationsValidator : AbstractValidator<SyncGitOrganizations>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncGitOrganizationsValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public SyncGitOrganizationsValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.GitStorageAccountId)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}
