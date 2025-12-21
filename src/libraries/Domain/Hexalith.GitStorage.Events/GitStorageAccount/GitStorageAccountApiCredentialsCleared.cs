// <copyright file="GitStorageAccountApiCredentialsCleared.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitStorageAccount;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when API credentials are cleared from a GitStorageAccount.
/// </summary>
/// <param name="Id">The identifier of the GitStorageAccount.</param>
[PolymorphicSerialization]
public partial record GitStorageAccountApiCredentialsCleared(string Id) : GitStorageAccountEvent(Id);
