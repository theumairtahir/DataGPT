using Dapper;
using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Models;
using DataGPT.Net.SqlServer.Exception;
using DataGPT.Net.SqlServer.Types.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataGPT.Net.SqlServer.Core;
internal class SqlSchemaFetcher : ISchemaFetcher
{
	private readonly IDbConnection _dbConnection;

	public SqlSchemaFetcher(IDbConnection dbConnection)
	{
		_dbConnection = dbConnection;
	}
	public async Task<DbSchema> GetSchemaAsync( )
	{
		try
		{
			DbSchema schema = new( );
			_dbConnection.Open( );
			var result = await _dbConnection.QueryAsync<ColumnSqlResult>(string.Format(SCHEMA_QUERY_FORMAT, nameof(ColumnSqlResult.ObjectId), nameof(ColumnSqlResult.TableName), nameof(ColumnSqlResult.ColumnName), nameof(ColumnSqlResult.DataType), nameof(ColumnSqlResult.IsView)));

			foreach (var table in result.GroupBy(x => (x.TableName, x.ObjectId, x.IsView)))
			{
				schema.Tables.Add(new DbTable
				{
					Name = table.Key.TableName,
					ObjectId = table.Key.ObjectId.ToString( ),
					IsView = table.Key.IsView,
					Columns = table.Select(x => new DbColumn { DataType = x.DataType, Name = x.ColumnName }).ToList( )
				});
			}
			return schema;
		}
		catch (SqlException ex)
		{
			throw new UncompleteDbActionException(ex);
		}
		finally
		{
			_dbConnection.Close( );
		}
	}



	private const string SCHEMA_QUERY_FORMAT = @"SELECT O.object_id AS {0},
       O.name AS {1},
       C.name AS {2},
       T.name AS {3},
       (CASE
            WHEN O.type_desc = 'VIEW' THEN
                1
            ELSE
                0
        END
       ) {4}
FROM sys.objects AS O
    JOIN sys.columns AS C
        ON O.object_id = C.object_id
    JOIN sys.types AS T
        ON C.system_type_id = T.system_type_id
WHERE O.type_desc IN ( 'USER_TABLE', 'VIEW' )
      AND T.name <> 'sysname'
ORDER BY O.name;";
}
