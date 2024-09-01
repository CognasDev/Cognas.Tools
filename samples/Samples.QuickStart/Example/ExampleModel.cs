using Cognas.ApiTools.SourceGenerators.Attributes;

namespace Samples.QuickStart.Example;

/// <summary>
/// 
/// </summary>
[IncludeInModelIdService]
[CommandScaffold(typeof(ExampleRequest), typeof(ExampleResponse), 1, false, true)]
[QueryScaffold(typeof(ExampleResponse), 1, true)]
public sealed record ExampleModel
{
    /// <summary>
    /// 
    /// </summary>
    [Id]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }
}