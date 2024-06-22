using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Cognas.ApiTools.Data;

/// <summary>
/// 
/// </summary>
public sealed class DatabaseConnectionFactory : IDatabaseConnectionFactory
{
    #region Field Declarations

    private readonly string _connectionString;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="DatabaseConnectionFactory"/>
    /// </summary>
    /// <param name="configuration"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public DatabaseConnectionFactory(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        _connectionString = configuration.GetConnectionString("localdb") ?? throw new KeyNotFoundException();
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IDbConnection Create() => new SqlConnection(_connectionString);

    #endregion
}