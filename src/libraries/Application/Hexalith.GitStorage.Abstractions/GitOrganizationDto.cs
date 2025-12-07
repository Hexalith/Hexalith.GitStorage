// <copyright file="GitOrganizationDto.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage;

using System.Runtime.Serialization;

/// <summary>
/// Represents an organization data transfer object from the Git provider adapter.
/// </summary>
/// <param name="Id">The unique identifier of the organization on the remote server.</param>
/// <param name="Name">The organization name (login).</param>
/// <param name="Description">Optional description of the organization.</param>
[DataContract]
public sealed record GitOrganizationDto(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description);
