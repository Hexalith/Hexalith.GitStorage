// <copyright file="GetMyToDoExports.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Requests.MyToDo;

using System.Runtime.Serialization;

using Hexalith.Application.Requests;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get MyToDo summaries with pagination.
/// </summary>
/// <param name="Skip">The number of MyToDo summaries to skip.</param>
/// <param name="Take">The number of MyToDo summaries to take.</param>
/// <param name="Results">The list of MyToDo summaries.</param>
[PolymorphicSerialization]
public partial record GetMyToDoExports(
    [property: DataMember(Order = 1)] int Skip,
    [property: DataMember(Order = 2)] int Take,
    [property: DataMember(Order = 3)] IEnumerable<MyToDo> Results)
    : IChunkableRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyToDoExports"/> class.
    /// </summary>
    public GetMyToDoExports()
        : this(0, 0, [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyToDoExports"/> class with specified skip and take values.
    /// </summary>
    /// <param name="skip">The number of MyToDo summaries to skip.</param>
    /// <param name="take">The number of MyToDo summaries to take.</param>
    public GetMyToDoExports(int skip, int take)
        : this(skip, take, [])
    {
    }

    /// <summary>
    /// Gets the aggregate ID of the MyToDo command.
    /// </summary>
    public static string AggregateId => MyToDoDomainHelper.MyToDoAggregateName;

    /// <summary>
    /// Gets the aggregate name of the MyToDo command.
    /// </summary>
    public static string AggregateName => MyToDoDomainHelper.MyToDoAggregateName;

    /// <inheritdoc/>
    IEnumerable<object>? ICollectionRequest.Results => Results;

    /// <inheritdoc/>
    public IChunkableRequest CreateNextChunkRequest()
        => ((IChunkableRequest)this).HasNextChunk
            ? this with { Skip = Skip + Take, Results = [] }
            : throw new InvalidRequestChunkException();

    /// <inheritdoc/>
    public ICollectionRequest CreateResults(IEnumerable<object> results) => this with { Results = (IEnumerable<MyToDo>)results };
}