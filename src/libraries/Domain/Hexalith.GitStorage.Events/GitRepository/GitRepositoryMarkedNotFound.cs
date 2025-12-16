// <copyright file="GitRepositoryMarkedNotFound.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitRepository;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a GitRepository is marked as not found on the remote server during sync.
/// </summary>
/// <param name="Id">The identifier of the GitRepository.</param>
/// <param name="MarkedAt">The timestamp when the repository was marked as not found.</param>
[PolymorphicSerialization]
public partial record GitRepositoryMarkedNotFound(
    string Id,
    [property: DataMember(Order = 2)] DateTimeOffset MarkedAt)
    : GitRepositoryEvent(Id);
