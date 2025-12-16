// <copyright file="GitRepositoryVisibilityChanged.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitRepository;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a GitRepository visibility setting is changed.
/// </summary>
/// <param name="Id">The identifier of the GitRepository.</param>
/// <param name="Visibility">The new visibility setting.</param>
[PolymorphicSerialization]
public partial record GitRepositoryVisibilityChanged(
    string Id,
    [property: DataMember(Order = 2)] GitRepositoryVisibility Visibility)
    : GitRepositoryEvent(Id);
