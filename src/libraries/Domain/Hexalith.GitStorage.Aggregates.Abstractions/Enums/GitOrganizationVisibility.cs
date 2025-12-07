// <copyright file="GitOrganizationVisibility.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Specifies the visibility level of a Git Organization.
/// </summary>
public enum GitOrganizationVisibility
{
    /// <summary>
    /// Organization is visible to everyone.
    /// </summary>
    Public = 0,

    /// <summary>
    /// Organization is only visible to members.
    /// </summary>
    Private = 1,

    /// <summary>
    /// Organization is visible to all authenticated users within the enterprise.
    /// Maps to GitHub 'internal' and Forgejo 'limited'.
    /// </summary>
    Internal = 2,
}
