using Cognas.ApiTools.Shared.Services;
using FluentAssertions;

namespace Shared.UnitTests.Services;

/// <summary>
/// 
/// </summary>
public sealed class PluralsServiceTests
{
    #region Unit Test Method Declarations

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void PluraliseModelName_Goose()
    {
        IPluralsService pluralsService = PluralsService.Instance;
        const string expectedValue = "Geese";
        string actualValue = pluralsService.PluraliseModelName<Goose>();
        actualValue.Should().Be(expectedValue);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void PluraliseModelName_Item()
    {
        IPluralsService pluralsService = PluralsService.Instance;
        const string expectedValue = "Items";
        string actualValue = pluralsService.PluraliseModelName<Item>();
        actualValue.Should().Be(expectedValue);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void PluraliseModelName_Man()
    {
        IPluralsService pluralsService = PluralsService.Instance;
        const string expectedValue = "Men";
        string actualValue = pluralsService.PluraliseModelName<Man>();
        actualValue.Should().Be(expectedValue);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void PluraliseModelName_Model()
    {
        IPluralsService pluralsService = PluralsService.Instance;
        const string expectedValue = "Models";
        string actualValue = pluralsService.PluraliseModelName<Model>();
        actualValue.Should().Be(expectedValue);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void PluraliseModelName_Thesis()
    {
        IPluralsService pluralsService = PluralsService.Instance;
        const string expectedValue = "Theses";
        string actualValue = pluralsService.PluraliseModelName<Thesis>();
        actualValue.Should().Be(expectedValue);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void PluraliseModelName_Tooth()
    {
        IPluralsService pluralsService = PluralsService.Instance;
        const string expectedValue = "Teeth";
        string actualValue = pluralsService.PluraliseModelName<Tooth>();
        actualValue.Should().Be(expectedValue);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void PluraliseModelName_Woman()
    {
        IPluralsService pluralsService = PluralsService.Instance;
        const string expectedValue = "Women";
        string actualValue = pluralsService.PluraliseModelName<Woman>();
        actualValue.Should().Be(expectedValue);
    }

    #endregion

    #region Test Helper Classes

    /// <summary>
    /// 
    /// </summary>
    private sealed record Goose { }

    /// <summary>
    /// 
    /// </summary>
    private sealed record Item { }

    /// <summary>
    /// 
    /// </summary>
    private sealed record Man { }

    /// <summary>
    /// 
    /// </summary>
    private sealed record Model { }

    /// <summary>
    /// 
    /// </summary>
    private sealed record Thesis { }

    /// <summary>
    /// 
    /// </summary>
    private sealed record Tooth { }

    /// <summary>
    /// 
    /// </summary>
    private sealed record Woman { }

    #endregion
}