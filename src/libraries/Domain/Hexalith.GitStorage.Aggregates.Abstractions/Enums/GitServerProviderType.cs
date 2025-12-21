// <copyright file="GitServerProviderType.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Represents the type of Git server platform.
/// </summary>
public enum GitServerProviderType
{
    /// <summary>
    /// GitHub.com or GitHub Enterprise Server.
    /// </summary>
    GitHub = 0,

    /// <summary>
    /// Forgejo Git server instances.
    /// </summary>
    Forgejo = 1,

    /// <summary>
    /// Gitea Git server instances.
    /// </summary>
    Gitea = 2,

    /// <summary>
    /// Generic Git server with compatible API.
    /// </summary>
    Generic = 3,
}
