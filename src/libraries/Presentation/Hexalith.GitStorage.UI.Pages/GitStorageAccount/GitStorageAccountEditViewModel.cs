// <copyright file="GitStorageAccountEditViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.UI.Pages.GitStorageAccount;

using System.Security.Claims;

using Hexalith.Application.Commands;
using Hexalith.Domains.ValueObjects;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Commands.GitStorageAccount;
using Hexalith.GitStorage.Requests.GitStorageAccount;
using Hexalith.UI.Components;

/// <summary>
/// ViewModel for editing file types.
/// </summary>
public sealed class GitStorageAccountEditViewModel : IIdDescription, IEntityViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitStorageAccountEditViewModel"/> class.
    /// </summary>
    /// <param name="details">The details of the file type.</param>
    public GitStorageAccountEditViewModel(GitStorageAccountDetailsViewModel details)
    {
        ArgumentNullException.ThrowIfNull(details);
        Id = details.Id;
        Original = details;
        Name = details.Name;
        Comments = details.Comments;
        Disabled = details.Disabled;
        ServerUrl = details.ServerUrl;
        AccessToken = details.AccessToken;
        ProviderType = details.ProviderType ?? GitServerProviderType.GitHub;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitStorageAccountEditViewModel"/> class.
    /// </summary>
    public GitStorageAccountEditViewModel()
    : this(new GitStorageAccountDetailsViewModel(
            string.Empty,
            string.Empty,
            string.Empty,
            false))
    {
    }

    /// <summary>
    /// Gets or sets the access token for the Git server API.
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets a value indicating whether the API credentials have changed.
    /// </summary>
    public bool ApiCredentialsChanged =>
        ServerUrl != Original.ServerUrl ||
        AccessToken != Original.AccessToken ||
        ProviderType != (Original.ProviderType ?? GitServerProviderType.GitHub);

    /// <summary>
    /// Gets or sets the description of the file type.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Gets a value indicating whether the description has changed.
    /// </summary>
    public bool DescriptionChanged => Comments != Original.Comments || Name != Original.Name;

    /// <summary>
    /// Gets or sets a value indicating whether the file type is disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets a value indicating whether there are changes in the file type details.
    /// </summary>
    public bool HasChanges =>
        Id != Original.Id ||
        DescriptionChanged ||
        Disabled != Original.Disabled ||
        ApiCredentialsChanged;

    /// <summary>
    /// Gets or sets the ID of the file type.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the file type.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the original details of the file type.
    /// </summary>
    public GitStorageAccountDetailsViewModel Original { get; }

    /// <summary>
    /// Gets or sets the type of Git server provider.
    /// </summary>
    public GitServerProviderType ProviderType { get; set; }

    /// <summary>
    /// Gets or sets the base URL of the Git server API.
    /// </summary>
    public string? ServerUrl { get; set; }

    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <summary>
    /// Saves the file type details asynchronously.
    /// </summary>
    /// <param name="user">The user performing the save operation.</param>
    /// <param name="commandService">The command service to submit commands.</param>
    /// <param name="create">A value indicating whether to create a new file type.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    internal async Task SaveAsync(ClaimsPrincipal user, ICommandService commandService, bool create, CancellationToken cancellationToken)
    {
        GitStorageAccountCommand gitStorageCommand;
        if (create)
        {
            gitStorageCommand = new AddGitStorageAccount(
                        Id!,
                        Name!,
                        Comments,
                        ServerUrl,
                        AccessToken,
                        string.IsNullOrEmpty(ServerUrl) ? null : ProviderType);
            await commandService.SubmitCommandAsync(user, gitStorageCommand, cancellationToken).ConfigureAwait(false);
            return;
        }

        if (DescriptionChanged)
        {
            gitStorageCommand = new ChangeGitStorageAccountDescription(
            Id!,
            Name!,
            Comments);
            await commandService.SubmitCommandAsync(user, gitStorageCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && Disabled)
        {
            gitStorageCommand = new DisableGitStorageAccount(Id);
            await commandService.SubmitCommandAsync(user, gitStorageCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && !Disabled)
        {
            gitStorageCommand = new EnableGitStorageAccount(Id);
            await commandService.SubmitCommandAsync(user, gitStorageCommand, cancellationToken).ConfigureAwait(false);
        }

        if (ApiCredentialsChanged)
        {
            if (!string.IsNullOrEmpty(ServerUrl) && !string.IsNullOrEmpty(AccessToken))
            {
                gitStorageCommand = new ChangeGitStorageAccountApiCredentials(
                    Id!,
                    ServerUrl!,
                    AccessToken!,
                    ProviderType);
                await commandService.SubmitCommandAsync(user, gitStorageCommand, cancellationToken).ConfigureAwait(false);
            }
            else if (string.IsNullOrEmpty(ServerUrl) && string.IsNullOrEmpty(AccessToken) && Original.HasApiCredentials)
            {
                gitStorageCommand = new ClearGitStorageAccountApiCredentials(Id!);
                await commandService.SubmitCommandAsync(user, gitStorageCommand, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}