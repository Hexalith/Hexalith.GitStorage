// <copyright file="GitStorageModulePolicies.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Helpers;

using System.Collections.Generic;

using Hexalith.Application;
using Hexalith.GitStorage;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Provides authorization policies for the GitStorage module.
/// </summary>
public static class GitStorageModulePolicies
{
    /// <summary>
    /// Gets the authorization policies for the GitStorage module.
    /// </summary>
    public static IDictionary<string, AuthorizationPolicy> AuthorizationPolicies =>
    new Dictionary<string, AuthorizationPolicy>
    {
        {
            GitStoragePolicies.Owner, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, GitStorageRoles.Owner)
                .Build()
        },
        {
            GitStoragePolicies.Contributor, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, GitStorageRoles.Owner, GitStorageRoles.Contributor)
                .Build()
        },
        {
            GitStoragePolicies.Reader, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, GitStorageRoles.Owner, GitStorageRoles.Contributor, GitStorageRoles.Reader)
                .Build()
        },
    };

    /// <summary>
    /// Adds the GitStorage module policies to the specified authorization options.
    /// </summary>
    /// <param name="options">The authorization options to add the policies to.</param>
    /// <returns>The updated authorization options.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the options parameter is null.</exception>
    public static AuthorizationOptions AddGitStorageAuthorizationPolicies(this AuthorizationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        foreach (KeyValuePair<string, AuthorizationPolicy> policy in AuthorizationPolicies)
        {
            options.AddPolicy(policy.Key, policy.Value);
        }

        return options;
    }
}