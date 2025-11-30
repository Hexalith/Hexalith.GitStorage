// <copyright file="GetGitStorageAccountExports.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Requests.GitStorageAccount;

using System.Runtime.Serialization;

using Hexalith.Application.Requests;
using Hexalith.GitStorage.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get GitStorageAccount summaries with pagination.
/// </summary>
/// <param name="Skip">The number of GitStorageAccount summaries to skip.</param>
/// <param name="Take">The number of GitStorageAccount summaries to take.</param>
/// <param name="Results">The list of GitStorageAccount summaries.</param>
[PolymorphicSerialization]
public partial record GetGitStorageAccountExports(
    [property: DataMember(Order = 1)] int Skip,
    [property: DataMember(Order = 2)] int Take,
    [property: DataMember(Order = 3)] IEnumerable<GitStorageAccount> Results)
    : IChunkableRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetGitStorageAccountExports"/> class.
    /// </summary>
    public GetGitStorageAccountExports()
        : this(0, 0, [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGitStorageAccountExports"/> class with specified skip and take values.
    /// </summary>
    /// <param name="skip">The number of GitStorageAccount summaries to skip.</param>
    /// <param name="take">The number of GitStorageAccount summaries to take.</param>
    public GetGitStorageAccountExports(int skip, int take)
        : this(skip, take, [])
    {
    }

    /// <summary>
    /// Gets the aggregate ID of the GitStorageAccount command.
    /// </summary>
    public static string AggregateId => GitStorageAccountDomainHelper.GitStorageAccountAggregateName;

    /// <summary>
    /// Gets the aggregate name of the GitStorageAccount command.
    /// </summary>
    public static string AggregateName => GitStorageAccountDomainHelper.GitStorageAccountAggregateName;

    /// <inheritdoc/>
    IEnumerable<object>? ICollectionRequest.Results => Results;

    /// <inheritdoc/>
    public IChunkableRequest CreateNextChunkRequest()
        => ((IChunkableRequest)this).HasNextChunk
            ? this with { Skip = Skip + Take, Results = [] }
            : throw new InvalidRequestChunkException();

    /// <inheritdoc/>
    public ICollectionRequest CreateResults(IEnumerable<object> results) => this with { Results = (IEnumerable<GitStorageAccount>)results };
}