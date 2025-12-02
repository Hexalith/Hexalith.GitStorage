// <copyright file="GetGitOrganizationDetails.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Requests.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.Application.Requests;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get the details of a GitOrganization by its ID.
/// </summary>
/// <param name="Id">The ID of the GitOrganization.</param>
/// <param name="Result">The GitOrganization details view model result.</param>
[PolymorphicSerialization]
public partial record GetGitOrganizationDetails(string Id, [property: DataMember(Order = 2)] GitOrganizationDetailsViewModel? Result = null)
    : GitOrganizationRequest(Id), IRequest
{
    /// <inheritdoc/>
    object? IRequest.Result => Result;
}
