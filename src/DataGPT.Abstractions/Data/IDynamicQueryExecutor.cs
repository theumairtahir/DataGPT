namespace DataGPT.Net.Abstractions.Data;

public interface IDynamicQueryExecutor
{
	IEnumerable<dynamic> ExecuteQuery(string query, IDbConfiguration dbConfiguration);
}
