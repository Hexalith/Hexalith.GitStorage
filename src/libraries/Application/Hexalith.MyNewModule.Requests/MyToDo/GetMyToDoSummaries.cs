// <copyright file="GetMyToDoSummaries.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Requests.MyToDo;

using System.Runtime.Serialization;

using Hexalith.Application.Requests;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get warehouse summaries with pagination.
/// </summary>
/// <param name="Skip">The number of warehouse summaries to skip.</param>
/// <param name="Take">The number of warehouse summaries to take.</param>
/// <param name="Search">Optional search term to filter results.</param>
/// <param name="Ids">The list of warehouse IDs to search by.</param>
/// <param name="Results">The list of warehouse summaries.</param>
[PolymorphicSerialization]
public partial record GetMyToDoSummaries(
    [property: DataMember(Order = 1)] int Skip,
    [property: DataMember(Order = 2)] int Take,
    [property: DataMember(Order = 3)] string? Search,
    [property: DataMember(Order = 4)] IEnumerable<string> Ids,
    [property: DataMember(Order = 5)] IEnumerable<MyToDoSummaryViewModel> Results) : ISearchChunkableRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyToDoSummaries"/> class.
    /// </summary>
    public GetMyToDoSummaries()
        : this(0, 0, null, [], [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyToDoSummaries"/> class with specified skip, take, and search values.
    /// </summary>
    /// <param name="skip">Number of records to skip for pagination.</param>
    /// <param name="take">Maximum number of records to return.</param>
    /// <param name="search">Optional search term to filter results.</param>
    public GetMyToDoSummaries(int skip, int take, string? search = null)
        : this(skip, take, search, [], [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyToDoSummaries"/> class with specified warehouse IDs.
    /// </summary>
    /// <param name="ids">The list of warehouse IDs to search by.</param>
    public GetMyToDoSummaries(IEnumerable<string> ids)
        : this(0, 0, null, ids, [])
    {
    }

    /// <summary>
    /// Gets the aggregate ID.
    /// </summary>
    public static string AggregateId => MyToDoDomainHelper.MyToDoAggregateName;

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public static string AggregateName => MyToDoDomainHelper.MyToDoAggregateName;

    /// <inheritdoc/>
    IEnumerable<object>? ICollectionRequest.Results => Results;

    /// <inheritdoc/>
    public IChunkableRequest CreateNextChunkRequest() => this with { Skip = Skip + Take, Results = [] };

    /// <inheritdoc/>
    public ICollectionRequest CreateResults(IEnumerable<object> results)
        => this with { Results = (IEnumerable<MyToDoSummaryViewModel>)results };
}