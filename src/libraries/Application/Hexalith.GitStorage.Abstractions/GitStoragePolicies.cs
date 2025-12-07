// <copyright file="GitStoragePolicies.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage;

/// <summary>
/// Defines the policies for GitStorage security within the application.
/// </summary>
public static class GitStoragePolicies
{
    /// <summary>
    /// Policy for users who can contribute to GitStorage.
    /// </summary>
    public const string Contributor = GitStorageRoles.Contributor;

    /// <summary>
    /// Policy for users who own GitStorage.
    /// </summary>
    public const string Owner = GitStorageRoles.Owner;

    /// <summary>
    /// Policy for users who can read GitStorage.
    /// </summary>
    public const string Reader = GitStorageRoles.Reader;
}