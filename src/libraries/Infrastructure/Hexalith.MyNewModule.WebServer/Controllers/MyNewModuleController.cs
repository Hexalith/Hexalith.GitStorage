// <copyright file="MyNewModuleController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.WebServer.Controllers;

using Hexalith.Application.Requests;
using Hexalith.MyNewModule.Requests.MyToDo;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for handling document file operations.
/// </summary>
[ApiController]
[Route("MyToDo")]
[Authorize]
public class MyNewModuleController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestService _requestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModuleController"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="requestService">The request service.</param>
    public MyNewModuleController(
        IHttpContextAccessor httpContextAccessor,
        IRequestService requestService)
    {
        ArgumentNullException.ThrowIfNull(requestService);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
        _requestService = requestService;
    }

    /// <summary>
    /// Downloads the file with the specified document ID.
    /// </summary>
    /// <param name="documentId">The document ID.</param>
    /// <returns>The file to download.</returns>
    [HttpGet("doSomething/{documentId}")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Avoid on async disposable")]
    public async Task<IActionResult> DoSomethingAsync(string documentId)
    {
        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        MyToDoDetailsViewModel? document = (await _requestService
            .SubmitAsync(user, new GetMyToDoDetails(documentId), CancellationToken.None)
            .ConfigureAwait(false))?.Result;

        if (document == null)
        {
            return NotFound("Modules not found.");
        }

        return Ok();
    }
}