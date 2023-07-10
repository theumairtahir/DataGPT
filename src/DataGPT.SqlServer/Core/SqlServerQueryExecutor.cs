using Dapper;
using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.SqlServer.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataGPT.Net.SqlServer.Core;
internal class SqlServerQueryExecutor : IDynamicQueryExecutor
{
	private readonly IDbConnection _connection;

	public SqlServerQueryExecutor(IDbConnection connection)
	{
		_connection = connection;
	}

	public async Task<IEnumerable<dynamic>> ExecuteQueryAsync(string query)
	{
		if (query is null || !query.ToUpper( ).StartsWith("SELECT"))
			throw new InvalidSqlServerQueryException(query ?? "null");

		try
		{
			_connection.Open( );
			return await _connection.QueryAsync(query);

		}
		catch (SqlException ex)
		{
			throw new IncompleteDbActionException(ex);
		}
		finally
		{
			_connection.Close( );
		}
	}
}
