// <copyright file="GitRepositoryEditViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.UI.Pages.GitRepository;

using System.Security.Claims;

using Hexalith.Application.Commands;
using Hexalith.Domains.ValueObjects;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Commands.GitRepository;
using Hexalith.GitStorage.Requests.GitRepository;
using Hexalith.UI.Components;

/// <summary>
/// ViewModel for editing GitRepository entities.
/// </summary>
public sealed class GitRepositoryEditViewModel : IIdDescription, IEntityViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitRepositoryEditViewModel"/> class.
    /// </summary>
    /// <param name="details">The details of the GitRepository.</param>
    public GitRepositoryEditViewModel(GitRepositoryDetailsViewModel details)
    {
        ArgumentNullException.ThrowIfNull(details);
        Id = details.Id;
        Original = details;
        Name = details.Name;
        Comments = details.Description;
        Url = details.Url;
        DefaultBranch = details.DefaultBranch;
        OrganizationId = details.OrganizationId;
        Visibility = details.Visibility;
        Disabled = details.Disabled;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitRepositoryEditViewModel"/> class.
    /// </summary>
    public GitRepositoryEditViewModel()
        : this(GitRepositoryDetailsViewModel.Empty)
    {
    }

    /// <summary>
    /// Gets or sets the comments (description) of the GitRepository.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Gets or sets the default branch of the GitRepository.
    /// </summary>
    public string? DefaultBranch { get; set; }

    /// <summary>
    /// Gets a value indicating whether the description has changed.
    /// </summary>
    public bool DescriptionChanged => Comments != Original.Description;

    /// <summary>
    /// Gets or sets a value indicating whether the GitRepository is disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets a value indicating whether there are changes in the GitRepository details.
    /// </summary>
    public bool HasChanges =>
        Id != Original.Id ||
        DescriptionChanged ||
        VisibilityChanged ||
        DefaultBranchChanged ||
        Disabled != Original.Disabled;

    /// <summary>
    /// Gets or sets the ID of the GitRepository.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the GitRepository.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Git Organization identifier.
    /// </summary>
    public string OrganizationId { get; set; }

    /// <summary>
    /// Gets the original details of the GitRepository.
    /// </summary>
    public GitRepositoryDetailsViewModel Original { get; }

    /// <summary>
    /// Gets or sets the URL of the GitRepository.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the GitRepository.
    /// </summary>
    public GitRepositoryVisibility Visibility { get; set; }

    /// <summary>
    /// Gets a value indicating whether the visibility has changed.
    /// </summary>
    public bool VisibilityChanged => Visibility != Original.Visibility;

    /// <summary>
    /// Gets a value indicating whether the default branch has changed.
    /// </summary>
    public bool DefaultBranchChanged => DefaultBranch != Original.DefaultBranch;

    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <summary>
    /// Saves the GitRepository details asynchronously.
    /// </summary>
    /// <param name="user">The user performing the save operation.</param>
    /// <param name="commandService">The command service to submit commands.</param>
    /// <param name="create">A value indicating whether to create a new GitRepository.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    internal async Task SaveAsync(ClaimsPrincipal user, ICommandService commandService, bool create, CancellationToken cancellationToken)
    {
        GitRepositoryCommand gitRepositoryCommand;
        if (create)
        {
            gitRepositoryCommand = new AddGitRepository(
                Id!,
                Name!,
                Comments,
                OrganizationId!,
                Visibility,
                DefaultBranch);
            await commandService.SubmitCommandAsync(user, gitRepositoryCommand, cancellationToken).ConfigureAwait(false);
            return;
        }

        if (DescriptionChanged)
        {
            gitRepositoryCommand = new ChangeGitRepositoryDescription(
                Id!,
                Comments);
            await commandService.SubmitCommandAsync(user, gitRepositoryCommand, cancellationToken).ConfigureAwait(false);
        }

        if (VisibilityChanged)
        {
            gitRepositoryCommand = new ChangeGitRepositoryVisibility(
                Id!,
                Visibility);
            await commandService.SubmitCommandAsync(user, gitRepositoryCommand, cancellationToken).ConfigureAwait(false);
        }

        if (DefaultBranchChanged && !string.IsNullOrWhiteSpace(DefaultBranch))
        {
            gitRepositoryCommand = new ChangeGitRepositoryDefaultBranch(
                Id!,
                DefaultBranch);
            await commandService.SubmitCommandAsync(user, gitRepositoryCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && Disabled)
        {
            gitRepositoryCommand = new DisableGitRepository(Id);
            await commandService.SubmitCommandAsync(user, gitRepositoryCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && !Disabled)
        {
            gitRepositoryCommand = new EnableGitRepository(Id);
            await commandService.SubmitCommandAsync(user, gitRepositoryCommand, cancellationToken).ConfigureAwait(false);
        }
    }
}
