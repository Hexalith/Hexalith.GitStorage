// <copyright file="MyToDoEventHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.EventHandlers;

using FluentValidation;

using Hexalith.MyNewModule.Commands.MyToDo;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding mytodo event handlers to the service collection.
/// </summary>
public static class MyToDoEventHandlerHelper
{
    /// <summary>
    /// Adds the mytodo event validators to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoEventValidators(this IServiceCollection services)
            => services.AddTransient<IValidator<AddMyToDo>, AddMyToDoValidator>();
}