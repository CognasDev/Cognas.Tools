﻿namespace Samples.QuickStart.Example.v1;

/// <summary>
/// 
/// </summary>
public sealed record ExampleRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int? Id { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; init; }
}