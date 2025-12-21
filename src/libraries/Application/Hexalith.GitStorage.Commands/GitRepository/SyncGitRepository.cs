// <copyright file="SyncGitRepository.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitRepository;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to synchronize a GitRepository with the remote Git Server.
/// </summary>
/// <param name="Id">The GitRepository identifier.</param>
[PolymorphicSerialization]
public partial record SyncGitRepository(string Id)
    : GitRepositoryCommand(Id);
