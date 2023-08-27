using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Models;
using DataGPT.Net.Abstractions.Processing;
using DataGPT.Net.Abstractions.Types.Models;

namespace DataGPT.Net.FluentMappings.Core;

internal abstract class AbstractMappingsProvider : IMappingsProvider
{
	private DbSchema? schema;
	protected readonly ISchemaFetcher _schemaFetcher;
	protected AbstractMappingsProvider(ISchemaFetcher schemaFetcher)
	{
		_schemaFetcher = schemaFetcher;
	}
	public abstract Task<List<EntityMapping>> GetEntityMappingsAsync( );

	protected async Task<DbSchema> GetSchemaAsync( ) => schema ??= await _schemaFetcher.GetSchemaAsync( );
}
