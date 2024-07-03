using Cognas.Tools.Shared.Extensions;
using FluentAssertions;

namespace Shared.UnitTests.Extensions;

/// <summary>
/// 
/// </summary>
public sealed class CollectionExtensionsTests
{
    #region Unit Test Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    [Theory]
    [InlineData(1000)]
    public void FastForEach(int count)
    {
        List<int> list = [];
        for (int i = 0; i < count; i++)
        {
            list.Add(1);
        }
        int sum = 0;
        list.FastForEach(item => sum += item);
        sum.Should().Be(count);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    [Theory]
    [InlineData(1000)]
    public void FastForEach_Predicate(int count)
    {
        List<int> list = [];
        for (int i = 0; i < count; i++)
        {
            list.Add(1);
        }
        int sum = 0;
        list.FastForEach(item => false, item => sum += item);
        sum.Should().Be(0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    [Theory]
    [InlineData(1000)]
    public void FastFirstOrDefault_Found(int count)
    {
        Random random = new();
        int valueToFind = random.Next(0, count - 1);
        List<int> list = [];
        for (int i = 0; i < count; i++)
        {
            list.Add(i == valueToFind ? valueToFind : 0);
        }
        int? foundValue = list.FastFirstOrDefault(item => item == valueToFind);
        foundValue.Should().NotBeNull();
        foundValue.Should().Be(valueToFind);
        list[foundValue.Value].Should().Be(valueToFind);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    [Theory]
    [InlineData(1000)]
    public void FastFirstOrDefault_NotFound(int count)
    {
        const int valueToFind = 0;
        List<int?> list = [];
        for (int i = 0; i < count; i++)
        {
            list.Add(1);
        }
        int? foundValue = list.FastFirstOrDefault(item => item == valueToFind);
        foundValue.Should().BeNull();
    }

    #endregion
}