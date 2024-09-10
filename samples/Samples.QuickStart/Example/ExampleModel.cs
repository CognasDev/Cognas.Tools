using Cognas.ApiTools.SourceGenerators.Attributes;

namespace Samples.QuickStart.Example;

/// <summary>
/// 
/// </summary>
[IncludeInModelIdService]
[CommandScaffold(typeof(v1.ExampleRequest), typeof(v1.ExampleResponse), 1, true)]
[QueryScaffold(typeof(v1.ExampleResponse), 1, true)]
[QueryScaffold(typeof(v2.ExampleResponse), 2, true)]
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