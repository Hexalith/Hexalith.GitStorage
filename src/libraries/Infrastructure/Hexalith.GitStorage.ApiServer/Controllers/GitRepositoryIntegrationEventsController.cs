// <copyright file="GitRepositoryIntegrationEventsController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.ApiServer.Controllers;

using Dapr;

using Hexalith.Application.Events;
using Hexalith.Application.Projections;
using Hexalith.Application.States;
using Hexalith.GitStorage.Aggregates;
using Hexalith.Infrastructure.WebApis.Buses;
using Hexalith.Infrastructure.WebApis.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Controller responsible for handling GitRepository-related integration events and managing GitRepository processing workflows.
/// Implements the <see cref="EventIntegrationController" /> to process various GitRepository events and update corresponding projections.
/// </summary>
/// <seealso cref="EventIntegrationController" />
/// <param name="eventProcessor">The integration event processor responsible for handling incoming events.</param>
/// <param name="projectionProcessor">The projection processor that updates read models based on processed events.</param>
/// <param name="hostEnvironment">The host environment providing runtime environment information.</param>
/// <param name="logger">The logger instance for recording diagnostic information.</param>
[ApiController]
[Route("/api/GitStorage/events")]
[SwaggerTag("GitRepository Integration Events Receiver")]
public class GitRepositoryIntegrationEventsController(
    IIntegrationEventProcessor eventProcessor,
    IProjectionUpdateProcessor projectionProcessor,
    IHostEnvironment hostEnvironment,
    ILogger<GitRepositoryIntegrationEventsController> logger)
    : EventIntegrationController(eventProcessor, projectionProcessor, hostEnvironment, logger)
{
    /// <summary>
    /// Processes GitRepository events asynchronously.
    /// </summary>
    /// <param name="eventState">The event state containing the message payload and metadata for processing.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation result:
    /// - 200 OK if the event was processed successfully
    /// - 400 Bad Request if the event data is invalid
    /// - 500 Internal Server Error if processing fails.</returns>
    [EventBusTopic(GitRepositoryDomainHelper.GitRepositoryAggregateName)]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("GitRepository")]
    [SwaggerOperation(Summary = "Handles GitRepository events", Description = "Processes GitRepository events and updates projections accordingly.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid event data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the event.")]
    public async Task<ActionResult> HandleGitRepositoryEventsAsync(MessageState eventState)
         => await HandleEventAsync(
                eventState,
                GitRepositoryDomainHelper.GitRepositoryAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);
}
