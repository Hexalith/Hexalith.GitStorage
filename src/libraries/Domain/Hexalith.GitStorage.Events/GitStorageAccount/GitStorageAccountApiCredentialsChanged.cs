// <copyright file="GitStorageAccountApiCredentialsChanged.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitStorageAccount;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when API credentials are changed on a GitStorageAccount.
/// </summary>
/// <param name="Id">The identifier of the GitStorageAccount.</param>
/// <param name="ServerUrl">The base URL of the Git server API (must be HTTPS).</param>
/// <param name="AccessToken">The authentication token for the Git server API.</param>
/// <param name="ProviderType">The type of Git server platform.</param>
[PolymorphicSerialization]
public partial record GitStorageAccountApiCredentialsChanged(
    string Id,
    [property: DataMember(Order = 2)] string ServerUrl,
    [property: DataMember(Order = 3)] string AccessToken,
    [property: DataMember(Order = 4)] GitServerProviderType ProviderType)
    : GitStorageAccountEvent(Id);
