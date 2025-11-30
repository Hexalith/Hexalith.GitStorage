// <copyright file="MyToDo.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.MyNewModule.Events.MyToDo;

/// <summary>
/// Represents a mytodo.
/// </summary>
/// <param name="Id">The mytodo identifier.</param>
/// <param name="Name">The mytodo name.</param>
/// <param name="Comments">The mytodo description.</param>
/// <param name="Disabled">The mytodo disabled status.</param>
[DataContract]
public sealed record MyToDo(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 7)] bool Disabled) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyToDo"/> class.
    /// </summary>
    public MyToDo()
        : this(string.Empty, string.Empty, null, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyToDo"/> class with the specified event.
    /// </summary>
    /// <param name="added">The event that adds a mytodo.</param>
    public MyToDo(MyToDoAdded added)
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
    public string AggregateName => MyToDoDomainHelper.MyToDoAggregateName;

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is MyToDoEvent && domainEvent is not MyToDoEnabled or MyToDoDisabled && Disabled)
        {
            return ApplyResult.NotEnabled(this);
        }
        else if (!(this as IDomainAggregate).IsInitialized() && domainEvent is not MyToDoAdded)
        {
            return ApplyResult.NotInitialized(this);
        }
        else
        {
            return domainEvent switch
            {
                MyToDoAdded e => ApplyEvent(e),
                MyToDoDescriptionChanged e => ApplyEvent(e),
                MyToDoDisabled e => ApplyEvent(e),
                MyToDoEnabled e => ApplyEvent(e),
                MyToDoEvent => ApplyResult.NotImplemented(this),
                _ => ApplyResult.InvalidEvent(this, domainEvent),
            };
        }
    }

    private ApplyResult ApplyEvent(MyToDoAdded e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new MyToDo(e), [e])
        : ApplyResult.Error(this, "The MyToDo already exists.");

    private ApplyResult ApplyEvent(MyToDoDescriptionChanged e) => Comments == e.Comments && Name == e.Name
            ? ApplyResult.Error(this, "The MyToDo name and description are already set to the specified values.")
            : ApplyResult.Success(this with { Comments = e.Comments, Name = e.Name }, [e]);

    private ApplyResult ApplyEvent(MyToDoDisabled e) => Disabled
            ? ApplyResult.Error(this, "The MyToDo is already disabled.")
            : ApplyResult.Success(this with { Disabled = true }, [e]);

    private ApplyResult ApplyEvent(MyToDoEnabled e) => Disabled
            ? ApplyResult.Success(this with { Disabled = false }, [e])
            : ApplyResult.Error(this, "The MyToDo is already enabled.");
}