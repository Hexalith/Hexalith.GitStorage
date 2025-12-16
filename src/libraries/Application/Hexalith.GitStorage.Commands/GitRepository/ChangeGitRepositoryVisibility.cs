// <copyright file="ChangeGitRepositoryVisibility.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitRepository;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to change a GitRepository's visibility setting.
/// </summary>
/// <param name="Id">The GitRepository identifier.</param>
/// <param name="Visibility">The new visibility setting.</param>
[PolymorphicSerialization]
public partial record ChangeGitRepositoryVisibility(
    string Id,
    [property: DataMember(Order = 2)] GitRepositoryVisibility Visibility)
    : GitRepositoryCommand(Id);
