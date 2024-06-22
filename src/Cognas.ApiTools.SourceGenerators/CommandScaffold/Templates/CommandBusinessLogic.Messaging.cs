﻿using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Data.Command;
using Cognas.ApiTools.Messaging;
using Cognas.ApiTools.Shared;
using Microsoft.Extensions.Logging;

namespace {0};

/// <summary>
/// Auto generated by decoration using <see cref="Cognas.ApiTools.SourceGenerators.Attributes.CommandScaffoldAttribute"/>.
/// </summary>
#nullable enable
public sealed partial class {1}CommandBusinessLogic : CommandBusinessLogicBase<{2}>
{{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="{1}CommandBusinessLogic"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="modelIdService"></param>
    /// <param name="databaseService"></param>
    /// <param name="messagingService"></param>
    public {1}CommandBusinessLogic
    (
        ILogger<{1}CommandBusinessLogic> logger,
        IModelIdService modelIdService,
        ICommandDatabaseService databaseService,
        IModelMessagingService<{2}> messagingService
    )
    : base(logger, modelIdService, databaseService, messagingService)
    {{
    }}

    #endregion
}}