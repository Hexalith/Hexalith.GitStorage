// <copyright file="ChangeGitStorageAccountApiCredentialsValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitStorageAccount;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitStorageAccount;

/// <summary>
/// Validator for <see cref="ChangeGitStorageAccountApiCredentials"/> command.
/// </summary>
public class ChangeGitStorageAccountApiCredentialsValidator : AbstractValidator<ChangeGitStorageAccountApiCredentials>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeGitStorageAccountApiCredentialsValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public ChangeGitStorageAccountApiCredentialsValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
        _ = RuleFor(x => x.ServerUrl)
            .NotEmpty()
            .WithMessage("Server URL is required.")
            .Must(BeValidHttpsUrl)
            .WithMessage("Server URL must be a valid HTTPS URL.");
        _ = RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("Access token is required.");
        _ = RuleFor(x => x.ProviderType)
            .IsInEnum()
            .WithMessage("Invalid provider type.");
    }

    private static bool BeValidHttpsUrl(string url)
        => Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) && uri.Scheme == Uri.UriSchemeHttps;
}
