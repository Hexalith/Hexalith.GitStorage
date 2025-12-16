// <copyright file="GitRepositoryDefaultBranchChanged.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitRepository;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a GitRepository default branch is changed.
/// </summary>
/// <param name="Id">The identifier of the GitRepository.</param>
/// <param name="DefaultBranch">The new default branch name.</param>
[PolymorphicSerialization]
public partial record GitRepositoryDefaultBranchChanged(
    string Id,
    [property: DataMember(Order = 2)] string DefaultBranch)
    : GitRepositoryEvent(Id);
