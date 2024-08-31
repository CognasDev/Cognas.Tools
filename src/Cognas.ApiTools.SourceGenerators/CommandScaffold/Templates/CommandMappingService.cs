﻿using Cognas.ApiTools.Mapping;
using Model = {0};
using Request = {1};

namespace {2};

/// <summary>
/// Auto generated by decoration using <see cref="Cognas.ApiTools.SourceGenerators.Attributes.CommandScaffoldAttribute"/>.
/// </summary>
#nullable enable
public sealed partial class {3}CommandMappingService : CommandMappingServiceBase<Model, Request>
{{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="{3}CommandMappingService"/>
    /// </summary>
    public {3}CommandMappingService()
    {{
    }}

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public override Model RequestToModel(Request request)
    {{
        Model model = new()
        {{
            {4}
        }};
        return model;
    }

    #endregion
}}