using DataGPT.Abstractions.Types.Models;

namespace DataGPT.Abstractions.Data;
public interface ISchemaFetcher
{
	public Task<List<DbTable>> GetSchemaAsync(IDbConfiguration dbConfiguration);
}
