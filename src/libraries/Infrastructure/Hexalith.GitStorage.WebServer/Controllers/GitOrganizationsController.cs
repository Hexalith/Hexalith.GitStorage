// <copyright file="GitOrganizationsController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.WebServer.Controllers;

using Hexalith.Application.Commands;
using Hexalith.Application.Requests;
using Hexalith.GitStorage.Commands.GitOrganization;
using Hexalith.GitStorage.Requests.GitOrganization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Controller for handling Git Organization operations.
/// </summary>
[ApiController]
[Route("api/git-organizations")]
[Authorize(Policy = GitStoragePolicies.Reader)]
public class GitOrganizationsController : ControllerBase
{
    private readonly ICommandService _commandService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestService _requestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganizationsController"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="requestService">The request service.</param>
    /// <param name="commandService">The command service.</param>
    public GitOrganizationsController(
        IHttpContextAccessor httpContextAccessor,
        IRequestService requestService,
        ICommandService commandService)
    {
        ArgumentNullException.ThrowIfNull(requestService);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(commandService);
        _httpContextAccessor = httpContextAccessor;
        _requestService = requestService;
        _commandService = commandService;
    }

    /// <summary>
    /// Gets a list of Git Organizations with pagination.
    /// </summary>
    /// <param name="skip">Number of records to skip.</param>
    /// <param name="take">Number of records to take.</param>
    /// <param name="search">Optional search term.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of Git Organization summaries.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "List Git Organizations", Description = "Retrieves a paginated list of Git Organizations.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of organizations.", typeof(IEnumerable<GitOrganizationSummaryViewModel>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
    public async Task<IActionResult> GetOrganizationsAsync(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        GetGitOrganizationSummaries? result = await _requestService
            .SubmitAsync(user, new GetGitOrganizationSummaries(skip, take, search), cancellationToken)
            .ConfigureAwait(false);

        return Ok(result?.Results ?? []);
    }

    /// <summary>
    /// Gets the details of a specific Git Organization.
    /// </summary>
    /// <param name="id">The organization identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Git Organization details.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get Git Organization details", Description = "Retrieves the details of a specific Git Organization.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the organization details.", typeof(GitOrganizationDetailsViewModel))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Organization not found.")]
    public async Task<IActionResult> GetOrganizationAsync(string id, CancellationToken cancellationToken = default)
    {
        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        GetGitOrganizationDetails? result = await _requestService
            .SubmitAsync(user, new GetGitOrganizationDetails(id), cancellationToken)
            .ConfigureAwait(false);

        if (result?.Result == null)
        {
            return NotFound("Git Organization not found.");
        }

        return Ok(result.Result);
    }

    /// <summary>
    /// Creates a new Git Organization.
    /// </summary>
    /// <param name="command">The command to create the organization.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created organization ID.</returns>
    [HttpPost]
    [Authorize(Policy = GitStoragePolicies.Contributor)]
    [SwaggerOperation(Summary = "Create Git Organization", Description = "Creates a new Git Organization.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Organization created successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User does not have permission.")]
    public async Task<IActionResult> CreateOrganizationAsync(
        [FromBody] AddGitOrganization command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        await _commandService.SubmitCommandAsync(user, command, cancellationToken).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetOrganizationAsync), new { id = command.Id }, new { id = command.Id });
    }

    /// <summary>
    /// Updates a Git Organization's description.
    /// </summary>
    /// <param name="id">The organization identifier.</param>
    /// <param name="command">The command to update the organization.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    [HttpPatch("{id}")]
    [Authorize(Policy = GitStoragePolicies.Contributor)]
    [SwaggerOperation(Summary = "Update Git Organization", Description = "Updates a Git Organization's description.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Organization updated successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data or ID mismatch.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User does not have permission.")]
    public async Task<IActionResult> UpdateOrganizationAsync(
        string id,
        [FromBody] ChangeGitOrganizationDescription command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        if (command.Id != id)
        {
            return BadRequest("The organization ID in the URL does not match the command.");
        }

        await _commandService.SubmitCommandAsync(user, command, cancellationToken).ConfigureAwait(false);

        return NoContent();
    }

    /// <summary>
    /// Disables a Git Organization.
    /// </summary>
    /// <param name="id">The organization identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    [HttpPost("{id}/disable")]
    [Authorize(Policy = GitStoragePolicies.Owner)]
    [SwaggerOperation(Summary = "Disable Git Organization", Description = "Disables a Git Organization locally.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Organization disabled successfully.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User does not have permission.")]
    public async Task<IActionResult> DisableOrganizationAsync(string id, CancellationToken cancellationToken = default)
    {
        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        await _commandService.SubmitCommandAsync(user, new DisableGitOrganization(id), cancellationToken).ConfigureAwait(false);

        return NoContent();
    }

    /// <summary>
    /// Enables a previously disabled Git Organization.
    /// </summary>
    /// <param name="id">The organization identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    [HttpPost("{id}/enable")]
    [Authorize(Policy = GitStoragePolicies.Owner)]
    [SwaggerOperation(Summary = "Enable Git Organization", Description = "Enables a previously disabled Git Organization.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Organization enabled successfully.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User does not have permission.")]
    public async Task<IActionResult> EnableOrganizationAsync(string id, CancellationToken cancellationToken = default)
    {
        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        await _commandService.SubmitCommandAsync(user, new EnableGitOrganization(id), cancellationToken).ConfigureAwait(false);

        return NoContent();
    }

    /// <summary>
    /// Changes a Git Organization's visibility.
    /// </summary>
    /// <param name="id">The organization identifier.</param>
    /// <param name="command">The command to change the visibility.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    [HttpPatch("{id}/visibility")]
    [Authorize(Policy = GitStoragePolicies.Contributor)]
    [SwaggerOperation(Summary = "Change Git Organization Visibility", Description = "Changes a Git Organization's visibility level.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Organization visibility changed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data or ID mismatch.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User does not have permission.")]
    public async Task<IActionResult> ChangeVisibilityAsync(
        string id,
        [FromBody] ChangeGitOrganizationVisibility command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        if (command.Id != id)
        {
            return BadRequest("The organization ID in the URL does not match the command.");
        }

        await _commandService.SubmitCommandAsync(user, command, cancellationToken).ConfigureAwait(false);

        return NoContent();
    }
}
