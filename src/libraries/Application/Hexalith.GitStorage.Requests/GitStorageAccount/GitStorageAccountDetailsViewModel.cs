// <copyright file="GitStorageAccountDetailsViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Requests.GitStorageAccount;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domains.ValueObjects;
using Hexalith.Extensions.Helpers;
using Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Represents the details of a GitStorageAccount.
/// </summary>
/// <param name="Id">The GitStorageAccount identifier.</param>
/// <param name="Name">The GitStorageAccount name.</param>
/// <param name="Comments">The GitStorageAccount description.</param>
/// <param name="Disabled">The GitStorageAccount disabled status.</param>
/// <param name="ServerUrl">The API base URL of the Git server.</param>
/// <param name="AccessToken">The authentication token for the Git server API (internal use only).</param>
/// <param name="ProviderType">The type of Git server platform.</param>
[DataContract]
public sealed record GitStorageAccountDetailsViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 5)] bool Disabled,
    [property: DataMember(Order = 6)] string? ServerUrl = null,
    [property: DataMember(Order = 7), JsonIgnore] string? AccessToken = null,
    [property: DataMember(Order = 8)] GitServerProviderType? ProviderType = null) : IIdDescription
{
    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <summary>
    /// Gets a value indicating whether this account has API credentials configured.
    /// </summary>
    public bool HasApiCredentials => !string.IsNullOrEmpty(ServerUrl) && !string.IsNullOrEmpty(AccessToken);

    /// <summary>
    /// Gets the masked access token for display purposes.
    /// Shows first and last 2 characters with asterisks in between for tokens longer than 4 characters.
    /// For shorter tokens, shows just asterisks.
    /// Returns null if no access token is configured.
    /// </summary>
    public string? MaskedAccessToken
    {
        get
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                return null;
            }

            if (AccessToken.Length <= 4)
            {
                return new string('*', AccessToken.Length);
            }

            return AccessToken[..2] + new string('*', AccessToken.Length - 4) + AccessToken[^2..];
        }
    }

    /// <summary>
    /// Gets an empty GitStorageAccount details view model.
    /// </summary>
    /// <returns>An empty GitStorageAccount details view model.</returns>
    public static GitStorageAccountDetailsViewModel Empty => new(string.Empty, string.Empty, string.Empty, false);

    /// <summary>
    /// Creates a new GitStorageAccount details view model.
    /// </summary>
    /// <param name="id">The GitStorageAccount identifier.</param>
    /// <returns>A new GitStorageAccount details view model.</returns>
    public static GitStorageAccountDetailsViewModel Create(string? id)
        => new(string.IsNullOrWhiteSpace(id) ? UniqueIdHelper.GenerateUniqueStringId() : id, string.Empty, string.Empty, false);
}