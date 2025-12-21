// <copyright file="ChangeGitRepositoryDescription.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitRepository;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to change a GitRepository's description.
/// </summary>
/// <param name="Id">The GitRepository identifier.</param>
/// <param name="Description">The new description (can be null to clear).</param>
[PolymorphicSerialization]
public partial record ChangeGitRepositoryDescription(
    string Id,
    [property: DataMember(Order = 2)] string? Description)
    : GitRepositoryCommand(Id);
