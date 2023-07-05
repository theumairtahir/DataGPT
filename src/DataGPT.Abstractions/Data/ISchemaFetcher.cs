using DataGPT.Abstractions.Types.Models;

namespace DataGPT.Abstractions.Data;
public interface ISchemaFetcher
{
	public List<DbTable> GetSchema( );
}
