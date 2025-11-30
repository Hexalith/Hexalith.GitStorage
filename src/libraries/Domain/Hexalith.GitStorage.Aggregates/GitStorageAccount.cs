// <copyright file="GitStorageAccount.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.GitStorage.Events.GitStorageAccount;

/// <summary>
/// Represents a GitStorageAccount.
/// </summary>
/// <param name="Id">The GitStorageAccount identifier.</param>
/// <param name="Name">The GitStorageAccount name.</param>
/// <param name="Comments">The GitStorageAccount description.</param>
/// <param name="Disabled">The GitStorageAccount disabled status.</param>
[DataContract]
public sealed record GitStorageAccount(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 7)] bool Disabled) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitStorageAccount"/> class.
    /// </summary>
    public GitStorageAccount()
        : this(string.Empty, string.Empty, null, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitStorageAccount"/> class with the specified event.
    /// </summary>
    /// <param name="added">The event that adds a GitStorageAccount.</param>
    public GitStorageAccount(GitStorageAccountAdded added)
        : this(
            (added ?? throw new ArgumentNullException(nameof(added))).Id,
            added.Name,
            added.Comments,
            false)
    {
    }

    /// <inheritdoc/>
    public string AggregateId => Id;

    /// <inheritdoc/>
    public string AggregateName => GitStorageAccountDomainHelper.GitStorageAccountAggregateName;

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is GitStorageAccountEvent && domainEvent is not GitStorageAccountEnabled or GitStorageAccountDisabled && Disabled)
        {
            return ApplyResult.NotEnabled(this);
        }
        else if (!(this as IDomainAggregate).IsInitialized() && domainEvent is not GitStorageAccountAdded)
        {
            return ApplyResult.NotInitialized(this);
        }
        else
        {
            return domainEvent switch
            {
                GitStorageAccountAdded e => ApplyEvent(e),
                GitStorageAccountDescriptionChanged e => ApplyEvent(e),
                GitStorageAccountDisabled e => ApplyEvent(e),
                GitStorageAccountEnabled e => ApplyEvent(e),
                GitStorageAccountEvent => ApplyResult.NotImplemented(this),
                _ => ApplyResult.InvalidEvent(this, domainEvent),
            };
        }
    }

    private ApplyResult ApplyEvent(GitStorageAccountAdded e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new GitStorageAccount(e), [e])
        : ApplyResult.Error(this, "The GitStorageAccount already exists.");

    private ApplyResult ApplyEvent(GitStorageAccountDescriptionChanged e) => Comments == e.Comments && Name == e.Name
            ? ApplyResult.Error(this, "The GitStorageAccount name and description are already set to the specified values.")
            : ApplyResult.Success(this with { Comments = e.Comments, Name = e.Name }, [e]);

    private ApplyResult ApplyEvent(GitStorageAccountDisabled e) => Disabled
            ? ApplyResult.Error(this, "The GitStorageAccount is already disabled.")
            : ApplyResult.Success(this with { Disabled = true }, [e]);

    private ApplyResult ApplyEvent(GitStorageAccountEnabled e) => Disabled
            ? ApplyResult.Success(this with { Disabled = false }, [e])
            : ApplyResult.Error(this, "The GitStorageAccount is already enabled.");
}