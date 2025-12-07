// <copyright file="GitStorageAccountEventHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.EventHandlers;

using FluentValidation;

using Hexalith.GitStorage.Commands.GitStorageAccount;
using Hexalith.GitStorage.Events.GitStorageAccount;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding GitStorageAccount event handlers to the service collection.
/// </summary>
public static class GitStorageAccountEventHandlerHelper
{
    /// <summary>
    /// Adds the GitStorageAccount command validators to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitStorageAccountCommandValidators(this IServiceCollection services)
        => services
            .AddTransient<IValidator<AddGitStorageAccount>, AddGitStorageAccountValidator>()
            .AddTransient<IValidator<ChangeGitStorageAccountDescription>, ChangeGitStorageAccountDescriptionValidator>()
            .AddTransient<IValidator<DisableGitStorageAccount>, DisableGitStorageAccountValidator>()
            .AddTransient<IValidator<EnableGitStorageAccount>, EnableGitStorageAccountValidator>();

    /// <summary>
    /// Adds the GitStorageAccount event validators to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitStorageAccountEventValidators(this IServiceCollection services)
        => services
            .AddTransient<IValidator<GitStorageAccountAdded>, GitStorageAccountAddedValidator>()
            .AddTransient<IValidator<GitStorageAccountDescriptionChanged>, GitStorageAccountDescriptionChangedValidator>()
            .AddTransient<IValidator<GitStorageAccountDisabled>, GitStorageAccountDisabledValidator>()
            .AddTransient<IValidator<GitStorageAccountEnabled>, GitStorageAccountEnabledValidator>();
}