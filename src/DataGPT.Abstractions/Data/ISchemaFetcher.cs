using DataGPT.Net.Abstractions.Models;

namespace DataGPT.Net.Abstractions.Data;
public interface ISchemaFetcher
{
	public Task<DbSchema> GetSchemaAsync(IDbConfiguration dbConfiguration);
}
