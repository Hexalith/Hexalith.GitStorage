// <copyright file="GitStorageAccountsController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.WebServer.Controllers;

using Hexalith.Application.Commands;
using Hexalith.GitStorage.Commands.GitStorageAccount;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Controller for managing Git storage account operations.
/// </summary>
[ApiController]
[Route("api/git-storage-accounts")]
[Authorize(Policy = GitStoragePolicies.Reader)]
public class GitStorageAccountsController : ControllerBase
{
    private readonly ICommandService _commandService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitStorageAccountsController"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="commandService">The command service.</param>
    public GitStorageAccountsController(
        IHttpContextAccessor httpContextAccessor,
        ICommandService commandService)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(commandService);
        _httpContextAccessor = httpContextAccessor;
        _commandService = commandService;
    }

    /// <summary>
    /// Updates the API credentials for a Git storage account.
    /// </summary>
    /// <param name="id">The identifier of the Git storage account.</param>
    /// <param name="request">The API credentials request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>NoContent if successful, appropriate error otherwise.</returns>
    [HttpPut("{id}/api-credentials")]
    [Authorize(Policy = GitStoragePolicies.Owner)]
    [SwaggerOperation(
        Summary = "Update API credentials",
        Description = "Updates the Git server API credentials for the specified storage account.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "API credentials updated successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User does not have permission.")]
    public async Task<IActionResult> ChangeApiCredentialsAsync(
        string id,
        [FromBody] ChangeApiCredentialsRequest request,
        CancellationToken cancellationToken = default)
    {
        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        ArgumentNullException.ThrowIfNull(request);

        ChangeGitStorageAccountApiCredentials command = new(
            id,
            request.ServerUrl,
            request.AccessToken,
            request.ProviderType);

        await _commandService
            .SubmitCommandAsync(user, command, cancellationToken)
            .ConfigureAwait(false);

        return NoContent();
    }

    /// <summary>
    /// Clears the API credentials from a Git storage account.
    /// </summary>
    /// <param name="id">The identifier of the Git storage account.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>NoContent if successful, appropriate error otherwise.</returns>
    [HttpDelete("{id}/api-credentials")]
    [Authorize(Policy = GitStoragePolicies.Owner)]
    [SwaggerOperation(
        Summary = "Clear API credentials",
        Description = "Removes the Git server API credentials from the specified storage account.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "API credentials cleared successfully.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "User does not have permission.")]
    public async Task<IActionResult> ClearApiCredentialsAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        ClearGitStorageAccountApiCredentials command = new(id);

        await _commandService
            .SubmitCommandAsync(user, command, cancellationToken)
            .ConfigureAwait(false);

        return NoContent();
    }
}
