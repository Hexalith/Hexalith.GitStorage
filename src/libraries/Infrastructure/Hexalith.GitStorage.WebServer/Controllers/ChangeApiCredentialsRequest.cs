// <copyright file="ChangeApiCredentialsRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.WebServer.Controllers;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Request model for changing API credentials.
/// </summary>
/// <param name="ServerUrl">The base URL of the Git server API (must be HTTPS).</param>
/// <param name="AccessToken">The authentication token for the Git server API.</param>
/// <param name="ProviderType">The type of Git server platform.</param>
[DataContract]
public record ChangeApiCredentialsRequest(
    [property: DataMember(Order = 1)] string ServerUrl,
    [property: DataMember(Order = 2)] string AccessToken,
    [property: DataMember(Order = 3), JsonRequired] GitServerProviderType ProviderType);
