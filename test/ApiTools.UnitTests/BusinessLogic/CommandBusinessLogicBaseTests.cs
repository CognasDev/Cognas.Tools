using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Data.Command;
using Cognas.ApiTools.Messaging;
using Cognas.ApiTools.Shared;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiTools.UnitTests.BusinessLogic;

/// <summary>
/// 
/// </summary>
public sealed class CommandBusinessLogicBaseTests
{
    #region Unit Test Method Declarations

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void InsertStoredProcedure()
    {
        ILogger<TestCommandBusinessLogic> logger = Mock.Of<ILogger<TestCommandBusinessLogic>>();
        IModelIdService modelIdService = Mock.Of<IModelIdService>();
        ICommandDatabaseService commandDatabaseService = Mock.Of<ICommandDatabaseService>();
        IModelMessagingService<TestModel> messagingService = Mock.Of<IModelMessagingService<TestModel>>();
        Mock<TestCommandBusinessLogic> mockBusinessLogic = new(logger, modelIdService, commandDatabaseService, messagingService) { CallBase = true };
        CommandBusinessLogicBase<TestModel> commandBusinessLogicBase = mockBusinessLogic.Object;

        commandBusinessLogicBase.InsertStoredProcedure.Should().Be("[dbo].[TestModels_Insert]");
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void UpdateStoredProcedure()
    {
        ILogger<TestCommandBusinessLogic> logger = Mock.Of<ILogger<TestCommandBusinessLogic>>();
        IModelIdService modelIdService = Mock.Of<IModelIdService>();
        ICommandDatabaseService commandDatabaseService = Mock.Of<ICommandDatabaseService>();
        IModelMessagingService<TestModel> messagingService = Mock.Of<IModelMessagingService<TestModel>>();
        Mock<TestCommandBusinessLogic> mockBusinessLogic = new(logger, modelIdService, commandDatabaseService, messagingService) { CallBase = true };
        CommandBusinessLogicBase<TestModel> commandBusinessLogicBase = mockBusinessLogic.Object;

        commandBusinessLogicBase.UpdateStoredProcedure.Should().Be("[dbo].[TestModels_Update]");
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void DeleteStoredProcedure()
    {
        ILogger<TestCommandBusinessLogic> logger = Mock.Of<ILogger<TestCommandBusinessLogic>>();
        IModelIdService modelIdService = Mock.Of<IModelIdService>();
        ICommandDatabaseService commandDatabaseService = Mock.Of<ICommandDatabaseService>();
        IModelMessagingService<TestModel> messagingService = Mock.Of<IModelMessagingService<TestModel>>();
        Mock<TestCommandBusinessLogic> mockBusinessLogic = new(logger, modelIdService, commandDatabaseService, messagingService) { CallBase = true };
        CommandBusinessLogicBase<TestModel> commandBusinessLogicBase = mockBusinessLogic.Object;

        commandBusinessLogicBase.DeleteStoredProcedure.Should().Be("[dbo].[TestModels_Delete]");
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
    /// <param name="modelIdService"></param>
    /// <param name="commandDatabaseService"></param>
    /// <param name="messagingService"></param>
    public class TestCommandBusinessLogic(ILogger<TestCommandBusinessLogic> logger,
                                          IModelIdService modelIdService,
                                          ICommandDatabaseService commandDatabaseService,
                                          IModelMessagingService<TestModel> messagingService)
        : CommandBusinessLogicBase<TestModel>(logger, modelIdService, commandDatabaseService, messagingService)
    {
    }

    #endregion
}