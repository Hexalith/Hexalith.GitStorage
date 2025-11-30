// <copyright file="GitStorageAccountRoles.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage;

/// <summary>
/// Defines the roles for GitStorageAccount security within the application.
/// </summary>
public static class GitStorageAccountRoles
{
    /// <summary>
    /// Role for users who can contribute to GitStorageAccount.
    /// </summary>
    public const string Contributor = nameof(GitStorage) + nameof(Contributor);

    /// <summary>
    /// Role for users who own GitStorageAccount.
    /// </summary>
    public const string Owner = nameof(GitStorage) + nameof(Owner);

    /// <summary>
    /// Role for users who can read GitStorageAccount.
    /// </summary>
    public const string Reader = nameof(GitStorage) + nameof(Reader);
}