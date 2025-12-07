// <copyright file="ClearGitStorageAccountApiCredentials.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitStorageAccount;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to clear the API credentials from a GitStorageAccount.
/// </summary>
/// <param name="Id">The identifier of the GitStorageAccount.</param>
[PolymorphicSerialization]
public partial record ClearGitStorageAccountApiCredentials(string Id) : GitStorageAccountCommand(Id);
