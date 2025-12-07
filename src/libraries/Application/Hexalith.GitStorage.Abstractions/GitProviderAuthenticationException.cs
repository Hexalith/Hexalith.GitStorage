// <copyright file="GitProviderAuthenticationException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage;

/// <summary>
/// Exception thrown when Git provider authentication fails due to invalid or expired credentials.
/// </summary>
public class GitProviderAuthenticationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitProviderAuthenticationException"/> class.
    /// </summary>
    public GitProviderAuthenticationException()
        : base("Git provider authentication failed. Credentials may be invalid or expired.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitProviderAuthenticationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public GitProviderAuthenticationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitProviderAuthenticationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GitProviderAuthenticationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
