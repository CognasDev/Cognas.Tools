using Cognas.ApiTools.Data;
using FluentAssertions;

namespace Cognas.Tools.DataTests;

/// <summary>
/// 
/// </summary>
public sealed class DatabaseTransactionServiceTests
{
    #region Unit Test Method Declarations

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void ExecuteTransaction()
    {
        DatabaseTransactionService databaseTransactionService = new();
        Func<Task> action = async () =>
        {
            await databaseTransactionService.ExecuteTransactionAsync(() => Task.CompletedTask);
        };
        action.Should().NotThrowAsync();
    }

    #endregion
}