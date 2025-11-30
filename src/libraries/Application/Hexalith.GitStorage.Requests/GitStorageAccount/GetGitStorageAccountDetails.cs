// <copyright file="GetGitStorageAccountDetails.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Requests.GitStorageAccount;

using System.Runtime.Serialization;

using Hexalith.Application.Requests;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get the details of a warehouse by its ID.
/// </summary>
/// <param name="Id">The ID of the warehouse.</param>
/// <param name="Result">The warehouse details view model result.</param>
[PolymorphicSerialization]
public partial record GetGitStorageAccountDetails(string Id, [property: DataMember(Order = 2)] GitStorageAccountDetailsViewModel? Result = null)
    : GitStorageAccountRequest(Id), IRequest
{
    /// <inheritdoc/>
    object? IRequest.Result => Result;
}