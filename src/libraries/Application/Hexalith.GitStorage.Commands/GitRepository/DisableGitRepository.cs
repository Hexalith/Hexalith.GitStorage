// <copyright file="DisableGitRepository.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitRepository;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to disable a GitRepository.
/// </summary>
/// <param name="Id">The GitRepository identifier.</param>
[PolymorphicSerialization]
public partial record DisableGitRepository(string Id)
    : GitRepositoryCommand(Id);
