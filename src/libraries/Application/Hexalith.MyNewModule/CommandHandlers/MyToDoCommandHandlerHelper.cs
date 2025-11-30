// <copyright file="MyToDoCommandHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.CommandHandlers;

using Hexalith.Application.Commands;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.MyNewModule.Commands.MyToDo;
using Hexalith.MyNewModule.Events.MyToDo;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding mytodo command handlers to the service collection.
/// </summary>
public static class MyToDoCommandHandlerHelper
{
    /// <summary>
    /// Adds the mytodo command handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoCommandHandlers(this IServiceCollection services)
    {
        _ = services.TryAddSimpleInitializationCommandHandler<AddMyToDo>(
            c => new MyToDoAdded(
                c.Id,
                c.Name,
                c.Comments),
            ev => new MyToDo((MyToDoAdded)ev));

        _ = services.TryAddSimpleCommandHandler<ChangeMyToDoDescription>(
            c => new MyToDoDescriptionChanged(
                c.Id,
                c.Name,
                c.Comments));

        _ = services.TryAddSimpleCommandHandler<DisableMyToDo>(
            c => new MyToDoDisabled(c.Id));

        _ = services.TryAddSimpleCommandHandler<EnableMyToDo>(
            c => new MyToDoEnabled(c.Id));

        return services;
    }
}