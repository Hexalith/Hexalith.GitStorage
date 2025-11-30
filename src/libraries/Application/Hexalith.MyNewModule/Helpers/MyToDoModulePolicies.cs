// <copyright file="MyToDoModulePolicies.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Helpers;

using System.Collections.Generic;

using Hexalith.Application;
using Hexalith.MyNewModule;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Provides authorization policies for the MyToDo module.
/// </summary>
public static class MyToDoModulePolicies
{
    /// <summary>
    /// Gets the authorization policies for the MyToDo module.
    /// </summary>
    public static IDictionary<string, AuthorizationPolicy> AuthorizationPolicies =>
    new Dictionary<string, AuthorizationPolicy>
    {
        {
            MyToDoPolicies.Owner, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, MyToDoRoles.Owner)
                .Build()
        },
        {
            MyToDoPolicies.Contributor, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, MyToDoRoles.Owner, MyToDoRoles.Contributor)
                .Build()
        },
        {
            MyToDoPolicies.Reader, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, MyToDoRoles.Owner, MyToDoRoles.Contributor, MyToDoRoles.Reader)
                .Build()
        },
    };

    /// <summary>
    /// Adds the MyToDo module policies to the specified authorization options.
    /// </summary>
    /// <param name="options">The authorization options to add the policies to.</param>
    /// <returns>The updated authorization options.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the options parameter is null.</exception>
    public static AuthorizationOptions AddMyToDoAuthorizationPolicies(this AuthorizationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        foreach (KeyValuePair<string, AuthorizationPolicy> policy in AuthorizationPolicies)
        {
            options.AddPolicy(policy.Key, policy.Value);
        }

        return options;
    }
}