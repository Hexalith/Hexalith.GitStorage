// <copyright file="AddGitStorageAccountValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitStorageAccount;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.GitStorageAccount;

/// <summary>
/// Validator for GitStorageAccount.
/// </summary>
public class AddGitStorageAccountValidator : AbstractValidator<AddGitStorageAccount>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddGitStorageAccountValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public AddGitStorageAccountValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer[Labels.NameRequired]);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);

        // Conditional validation for API credentials
        _ = RuleFor(x => x.ServerUrl)
            .Must(BeValidHttpsUrl)
            .When(x => !string.IsNullOrEmpty(x.ServerUrl))
            .WithMessage("Server URL must be a valid HTTPS URL.");
        _ = RuleFor(x => x.AccessToken)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.ServerUrl))
            .WithMessage("Access token is required when server URL is provided.");
        _ = RuleFor(x => x.ProviderType)
            .IsInEnum()
            .When(x => x.ProviderType.HasValue)
            .WithMessage("Invalid provider type.");
    }

    private static bool BeValidHttpsUrl(string? url)
        => string.IsNullOrEmpty(url) || (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) && uri.Scheme == Uri.UriSchemeHttps);
}