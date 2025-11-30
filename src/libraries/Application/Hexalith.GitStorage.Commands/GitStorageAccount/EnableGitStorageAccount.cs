// <copyright file="EnableGitStorageAccount.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Commands.GitStorageAccount;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command raised when a GitStorageAccount is enabled.
/// </summary>
/// <param name="Id">The identifier of the GitStorageAccount.</param>
[PolymorphicSerialization]
public partial record EnableGitStorageAccount(string Id) : GitStorageAccountCommand(Id);