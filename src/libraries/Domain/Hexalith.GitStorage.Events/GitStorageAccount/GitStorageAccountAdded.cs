// <copyright file="GitStorageAccountAdded.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Events.GitStorageAccount;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a new GitStorageAccount is added.
/// </summary>
/// <param name="Id">The identifier of the GitStorageAccount.</param>
/// <param name="Name">The name of the GitStorageAccount.</param>
/// <param name="Comments">Optional comments about the GitStorageAccount.</param>
[PolymorphicSerialization]
public partial record GitStorageAccountAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : GitStorageAccountEvent(Id)
;