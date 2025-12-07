// <copyright file="GitStorageRoles.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage;

/// <summary>
/// Defines the roles for GitStorage security within the application.
/// </summary>
public static class GitStorageRoles
{
    /// <summary>
    /// Role for users who can contribute to GitStorage.
    /// </summary>
    public const string Contributor = nameof(GitStorage) + nameof(Contributor);

    /// <summary>
    /// Role for users who own GitStorage.
    /// </summary>
    public const string Owner = nameof(GitStorage) + nameof(Owner);

    /// <summary>
    /// Role for users who can read GitStorage.
    /// </summary>
    public const string Reader = nameof(GitStorage) + nameof(Reader);
}