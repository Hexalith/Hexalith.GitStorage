// <copyright file="GetMyToDoIds.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Requests.MyToDo;

using System.Runtime.Serialization;

using Hexalith.Application.Requests;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get warehouse IDs with pagination.
/// </summary>
/// <param name="Skip">The number of items to skip.</param>
/// <param name="Take">The number of items to take.</param>
/// <param name="Results">The collection of warehouse IDs.</param>
[PolymorphicSerialization]
public partial record GetMyToDoIds(
    [property: DataMember(Order = 1)] int Skip,
    [property: DataMember(Order = 2)] int Take,
    [property: DataMember(Order = 5)] IEnumerable<string> Results) : IChunkableRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyToDoIds"/> class.
    /// </summary>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="take">The number of items to take.</param>
    public GetMyToDoIds(int skip, int take)
        : this(skip, take, [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyToDoIds"/> class.
    /// </summary>
    public GetMyToDoIds()
        : this(0, 0, [])
    {
    }

    /// <inheritdoc/>
    IEnumerable<object>? ICollectionRequest.Results => Results;

    /// <inheritdoc/>
    public IChunkableRequest CreateNextChunkRequest() => this with { Skip = Skip + Take, Results = [] };

    /// <inheritdoc/>
    public ICollectionRequest CreateResults(IEnumerable<object> results) => this with { Results = results.Cast<string>() };
}