// <copyright file="GitStorageAccountApiCredentialsClearedValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitStorageAccount;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitStorageAccount;

/// <summary>
/// Validator for <see cref="GitStorageAccountApiCredentialsCleared"/> event.
/// </summary>
public class GitStorageAccountApiCredentialsClearedValidator : AbstractValidator<GitStorageAccountApiCredentialsCleared>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitStorageAccountApiCredentialsClearedValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public GitStorageAccountApiCredentialsClearedValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}
