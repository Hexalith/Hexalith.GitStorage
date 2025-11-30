// <copyright file="GitStorageAccountDetailsViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Requests.GitStorageAccount;

using System.Runtime.Serialization;

using Hexalith.Domains.ValueObjects;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Represents the details of a GitStorageAccount.
/// </summary>
/// <param name="Id">The GitStorageAccount identifier.</param>
/// <param name="Name">The GitStorageAccount name.</param>
/// <param name="Comments">The GitStorageAccount description.</param>
/// <param name="Disabled">The GitStorageAccount disabled status.</param>
[DataContract]
public sealed record GitStorageAccountDetailsViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 5)] bool Disabled) : IIdDescription
{
    /// <inheritdoc/>
    string IIdDescription.Description => Name;

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