// <copyright file="GitRepositoryDisabled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitRepository;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a GitRepository is disabled locally.
/// </summary>
/// <param name="Id">The identifier of the GitRepository.</param>
[PolymorphicSerialization]
public partial record GitRepositoryDisabled(string Id)
    : GitRepositoryEvent(Id);
