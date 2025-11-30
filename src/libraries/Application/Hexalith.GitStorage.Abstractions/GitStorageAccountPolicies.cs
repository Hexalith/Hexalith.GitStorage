// <copyright file="GitStorageAccountPolicies.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage;

/// <summary>
/// Defines the policies for GitStorageAccount security within the application.
/// </summary>
public static class GitStorageAccountPolicies
{
    /// <summary>
    /// Policy for users who can contribute to GitStorageAccount.
    /// </summary>
    public const string Contributor = GitStorageAccountRoles.Contributor;

    /// <summary>
    /// Policy for users who own GitStorageAccount.
    /// </summary>
    public const string Owner = GitStorageAccountRoles.Owner;

    /// <summary>
    /// Policy for users who can read GitStorageAccount.
    /// </summary>
    public const string Reader = GitStorageAccountRoles.Reader;
}