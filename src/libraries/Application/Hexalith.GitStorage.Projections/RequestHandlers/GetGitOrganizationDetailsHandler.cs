// <copyright file="GetGitOrganizationDetailsHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.RequestHandlers;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.GitStorage.Requests.GitOrganization;

/// <summary>
/// Handler for getting GitOrganization details.
/// </summary>
public class GetGitOrganizationDetailsHandler : RequestHandlerBase<GetGitOrganizationDetails>
{
    private readonly IProjectionFactory<GitOrganizationDetailsViewModel> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGitOrganizationDetailsHandler"/> class.
    /// </summary>
    /// <param name="factory">The GitOrganization projection factory.</param>
    /// <exception cref="ArgumentNullException">Thrown when factory is null.</exception>
    public GetGitOrganizationDetailsHandler(IProjectionFactory<GitOrganizationDetailsViewModel> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <inheritdoc/>
    public override async Task<GetGitOrganizationDetails> ExecuteAsync(GetGitOrganizationDetails request, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);

        return request with
        {
            Result = await _factory
                .GetStateAsync(metadata.AggregateGlobalId, cancellationToken)
                .ConfigureAwait(false)
                    ?? throw new InvalidOperationException($"GitOrganization {metadata.AggregateGlobalId} not found."),
        };
    }
}
