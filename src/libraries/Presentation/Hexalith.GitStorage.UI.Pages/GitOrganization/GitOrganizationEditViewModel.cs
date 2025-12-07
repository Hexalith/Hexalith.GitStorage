// <copyright file="GitOrganizationEditViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.UI.Pages.GitOrganization;

using System.Security.Claims;

using Hexalith.Application.Commands;
using Hexalith.Domains.ValueObjects;
using Hexalith.GitStorage.Commands.GitOrganization;
using Hexalith.GitStorage.Requests.GitOrganization;
using Hexalith.UI.Components;

/// <summary>
/// ViewModel for editing GitOrganization entities.
/// </summary>
public sealed class GitOrganizationEditViewModel : IIdDescription, IEntityViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganizationEditViewModel"/> class.
    /// </summary>
    /// <param name="details">The details of the GitOrganization.</param>
    public GitOrganizationEditViewModel(GitOrganizationDetailsViewModel details)
    {
        ArgumentNullException.ThrowIfNull(details);
        Id = details.Id;
        Original = details;
        Name = details.Name;
        Comments = details.Description;
        GitStorageAccountId = details.GitStorageAccountId;
        Disabled = details.Disabled;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganizationEditViewModel"/> class.
    /// </summary>
    public GitOrganizationEditViewModel()
        : this(GitOrganizationDetailsViewModel.Empty)
    {
    }

    /// <summary>
    /// Gets or sets the comments (description) of the GitOrganization.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Gets a value indicating whether the description has changed.
    /// </summary>
    public bool DescriptionChanged => Comments != Original.Description || Name != Original.Name;

    /// <summary>
    /// Gets or sets a value indicating whether the GitOrganization is disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the Git Storage Account identifier.
    /// </summary>
    public string GitStorageAccountId { get; set; }

    /// <summary>
    /// Gets a value indicating whether there are changes in the GitOrganization details.
    /// </summary>
    public bool HasChanges =>
        Id != Original.Id ||
        DescriptionChanged ||
        Disabled != Original.Disabled;

    /// <summary>
    /// Gets or sets the ID of the GitOrganization.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the GitOrganization.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the original details of the GitOrganization.
    /// </summary>
    public GitOrganizationDetailsViewModel Original { get; }

    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <summary>
    /// Saves the GitOrganization details asynchronously.
    /// </summary>
    /// <param name="user">The user performing the save operation.</param>
    /// <param name="commandService">The command service to submit commands.</param>
    /// <param name="create">A value indicating whether to create a new GitOrganization.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    internal async Task SaveAsync(ClaimsPrincipal user, ICommandService commandService, bool create, CancellationToken cancellationToken)
    {
        GitOrganizationCommand gitOrganizationCommand;
        if (create)
        {
            gitOrganizationCommand = new AddGitOrganization(
                Id!,
                Name!,
                Comments,
                GitStorageAccountId!);
            await commandService.SubmitCommandAsync(user, gitOrganizationCommand, cancellationToken).ConfigureAwait(false);
            return;
        }

        if (DescriptionChanged)
        {
            gitOrganizationCommand = new ChangeGitOrganizationDescription(
                Id!,
                Name!,
                Comments);
            await commandService.SubmitCommandAsync(user, gitOrganizationCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && Disabled)
        {
            gitOrganizationCommand = new DisableGitOrganization(Id);
            await commandService.SubmitCommandAsync(user, gitOrganizationCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && !Disabled)
        {
            gitOrganizationCommand = new EnableGitOrganization(Id);
            await commandService.SubmitCommandAsync(user, gitOrganizationCommand, cancellationToken).ConfigureAwait(false);
        }
    }
}
