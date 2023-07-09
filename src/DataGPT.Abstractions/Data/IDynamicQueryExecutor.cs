namespace DataGPT.Net.Abstractions.Data;

public interface IDynamicQueryExecutor
{
	Task<IEnumerable<dynamic>> ExecuteQueryAsync(string query);
}
