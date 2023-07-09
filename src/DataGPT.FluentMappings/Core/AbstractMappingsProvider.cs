using DataGPT.Abstractions.Data;
using DataGPT.Abstractions.Processing;
using DataGPT.Abstractions.Types.Models;
using DataGPT.FluentMappings.Constants;

namespace DataGPT.FluentMappings.Core;

internal abstract class AbstractMappingsProvider : IMappingsProvider
{
	private DbSchema? schema;
	protected readonly ISchemaFetcher _schemaFetcher;
	protected readonly IDbConfiguration _dbConfiguration;
	protected AbstractMappingsProvider(ISchemaFetcher schemaFetcher, IDbConfiguration dbConfiguration)
	{
		_schemaFetcher = schemaFetcher;
		_dbConfiguration = dbConfiguration;
	}
	public abstract Task<Dictionary<string, string>> GetColumnMappingsAsync(string entityName);
	public abstract Task<Dictionary<string, string>> GetEntityMappingsAsync( );

	protected async Task<DbSchema> GetSchemaAsync() => schema ??= await _schemaFetcher.GetSchemaAsync(_dbConfiguration);

	protected static Exception EntityNotPresentException(string entityName, string paramName) => new ArgumentException(string.Format(Errors.ENTITY_NOT_PRESENT_FORMAT, entityName), paramName);
}
