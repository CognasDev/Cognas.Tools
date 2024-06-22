using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Data.Query;
using Cognas.ApiTools.Shared;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiTools.UnitTests.BusinessLogic;

/// <summary>
/// 
/// </summary>
public sealed class QueryBusinessLogicBaseTests
{
    #region Unit Test Method Declarations

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void CacheKey()
    {
        ILogger<TestQueryBusinessLogic> logger = Mock.Of<ILogger<TestQueryBusinessLogic>>();
        IMemoryCache memoryCache = Mock.Of<IMemoryCache>();
        IModelIdService modelIdService = Mock.Of<IModelIdService>();
        IQueryDatabaseService queryDatabaseService = Mock.Of<IQueryDatabaseService>();

        Mock<TestQueryBusinessLogic> mockBusinessLogic = new(logger, memoryCache, modelIdService, queryDatabaseService) { CallBase = true };
        QueryBusinessLogicBase<TestModel> queryBusinessLogicBase = mockBusinessLogic.Object;
        queryBusinessLogicBase.CacheKey.Should().Be(nameof(TestModel));
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void CacheTimeOutMinutes()
    {
        ILogger<TestQueryBusinessLogic> logger = Mock.Of<ILogger<TestQueryBusinessLogic>>();
        IMemoryCache memoryCache = Mock.Of<IMemoryCache>();
        IModelIdService modelIdService = Mock.Of<IModelIdService>();
        IQueryDatabaseService queryDatabaseService = Mock.Of<IQueryDatabaseService>();

        Mock<TestQueryBusinessLogic> mockBusinessLogic = new(logger, memoryCache, modelIdService, queryDatabaseService) { CallBase = true };
        QueryBusinessLogicBase<TestModel> queryBusinessLogicBase = mockBusinessLogic.Object;
        queryBusinessLogicBase.CacheTimeOutMinutes.Should().Be(30);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void UseCache()
    {
        ILogger<TestQueryBusinessLogic> logger = Mock.Of<ILogger<TestQueryBusinessLogic>>();
        IMemoryCache memoryCache = Mock.Of<IMemoryCache>();
        IModelIdService modelIdService = Mock.Of<IModelIdService>();
        IQueryDatabaseService queryDatabaseService = Mock.Of<IQueryDatabaseService>();

        Mock<TestQueryBusinessLogic> mockBusinessLogic = new(logger, memoryCache, modelIdService, queryDatabaseService) { CallBase = true };
        QueryBusinessLogicBase<TestModel> queryBusinessLogicBase = mockBusinessLogic.Object;
        queryBusinessLogicBase.UseCache.Should().BeTrue();
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void SelectStoredProcedure()
    {
        ILogger<TestQueryBusinessLogic> logger = Mock.Of<ILogger<TestQueryBusinessLogic>>();
        IMemoryCache memoryCache = Mock.Of<IMemoryCache>();
        IModelIdService modelIdService = Mock.Of<IModelIdService>();
        IQueryDatabaseService queryDatabaseService = Mock.Of<IQueryDatabaseService>();

        Mock<TestQueryBusinessLogic> mockBusinessLogic = new(logger, memoryCache, modelIdService, queryDatabaseService) { CallBase = true };
        QueryBusinessLogicBase<TestModel> queryBusinessLogicBase = mockBusinessLogic.Object;
        queryBusinessLogicBase.SelectStoredProcedure.Should().Be("[dbo].[TestModels_Select]");
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void SelectByIdStoredProcedure()
    {
        ILogger<TestQueryBusinessLogic> logger = Mock.Of<ILogger<TestQueryBusinessLogic>>();
        IMemoryCache memoryCache = Mock.Of<IMemoryCache>();
        IModelIdService modelIdService = Mock.Of<IModelIdService>();
        IQueryDatabaseService queryDatabaseService = Mock.Of<IQueryDatabaseService>();

        Mock<TestQueryBusinessLogic> mockBusinessLogic = new(logger, memoryCache, modelIdService, queryDatabaseService) { CallBase = true };
        QueryBusinessLogicBase<TestModel> queryBusinessLogicBase = mockBusinessLogic.Object;
        queryBusinessLogicBase.SelectByIdStoredProcedure.Should().Be("[dbo].[TestModels_SelectById]");
    }

    #endregion

    #region Test Helper Classes

    /// <summary>
    /// 
    /// </summary>
    public sealed record TestModel { }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="memoryCache"></param>
    /// <param name="modelIdService"></param>
    /// <param name="queryDatabaseService"></param>
    public class TestQueryBusinessLogic(ILogger<TestQueryBusinessLogic> logger, IMemoryCache memoryCache, IModelIdService modelIdService, IQueryDatabaseService queryDatabaseService)
        : QueryBusinessLogicBase<TestModel>(logger, memoryCache, modelIdService, queryDatabaseService)
    {
    }

    #endregion
}