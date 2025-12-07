// <copyright file="IGitProviderAdapter.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage;

/// <summary>
/// Defines the contract for interacting with Git provider APIs (GitHub, Forgejo, etc.).
/// </summary>
public interface IGitProviderAdapter
{
    /// <summary>
    /// Lists all organizations accessible to the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A collection of organization DTOs.</returns>
    /// <exception cref="GitProviderAuthenticationException">Thrown when credentials are invalid or expired.</exception>
    Task<IEnumerable<GitOrganizationDto>> ListOrganizationsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets details of a specific organization by name.
    /// </summary>
    /// <param name="name">The organization name.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The organization DTO if found; otherwise null.</returns>
    /// <exception cref="GitProviderAuthenticationException">Thrown when credentials are invalid or expired.</exception>
    Task<GitOrganizationDto?> GetOrganizationAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new organization on the remote Git server.
    /// </summary>
    /// <param name="name">The organization name.</param>
    /// <param name="description">Optional description for the organization.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The created organization DTO.</returns>
    /// <exception cref="GitProviderAuthenticationException">Thrown when credentials are invalid or expired.</exception>
    Task<GitOrganizationDto> CreateOrganizationAsync(string name, string? description, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing organization on the remote Git server.
    /// </summary>
    /// <param name="name">The organization name.</param>
    /// <param name="description">The new description for the organization.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The updated organization DTO.</returns>
    /// <exception cref="GitProviderAuthenticationException">Thrown when credentials are invalid or expired.</exception>
    Task<GitOrganizationDto> UpdateOrganizationAsync(string name, string? description, CancellationToken cancellationToken);
}
