// <copyright file="GitOrganizationAdded.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a new GitOrganization is created via the application API.
/// </summary>
/// <param name="Id">The identifier of the GitOrganization (composite key).</param>
/// <param name="Name">The organization name as it appears on the Git Server.</param>
/// <param name="Description">Optional description of the organization.</param>
/// <param name="GitStorageAccountId">Reference to the parent GitStorageAccount entity.</param>
/// <param name="Visibility">The visibility level of the organization.</param>
/// <param name="WebUrl">The URL of the organization.</param>
/// <param name="GitAccountId">The account identifier on the Git server.</param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "JSerialization")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "JSerialization")]
[PolymorphicSerialization]
public partial record GitOrganizationAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] string GitStorageAccountId,
    [property: DataMember(Order = 5)] GitOrganizationVisibility Visibility = GitOrganizationVisibility.Public,
    [property: DataMember(Order = 6)] string? WebUrl = null,
    [property: DataMember(Order = 7)] string? GitAccountId = null)
    : GitOrganizationEvent(Id);