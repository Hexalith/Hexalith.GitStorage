// <copyright file="CommandBusProxy.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.WebApp.Services;

using System.Net.Http.Json;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a proxy for the command bus.
/// </summary>
public class CommandBusProxy : ICommandBus
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandBusProxy"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used for sending commands.</param>
    public CommandBusProxy(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task PublishAsync(object message, Metadata metadata, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _httpClient
            .PostAsJsonAsync("/api/command/publish", new MessageState((Polymorphic)message, metadata), PolymorphicHelper.DefaultJsonSerializerOptions, cancellationToken)
            .ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();
    }
}