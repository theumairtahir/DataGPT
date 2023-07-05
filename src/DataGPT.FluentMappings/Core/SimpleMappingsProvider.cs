using DataGPT.Abstractions.Data;
using DataGPT.Abstractions.Processing;
using DataGPT.Abstractions.Types.Models;
using DataGPT.FluentMappings.Constants;

namespace DataGPT.FluentMappings.Core;
internal class SimpleMappingsProvider : IMappingsProvider
{
	private readonly ISchemaFetcher _schemaFetcher;
	private readonly IDbConfiguration _dbConfiguration;
	private DbSchema? schema;

	public SimpleMappingsProvider(ISchemaFetcher schemaFetcher, IDbConfiguration dbConfiguration)
	{
		_schemaFetcher = schemaFetcher;
		_dbConfiguration = dbConfiguration;
	}

	public async Task<Dictionary<string, string>> GetColumnMappingsAsync(string entityName)
	{
		schema ??= await _schemaFetcher.GetSchemaAsync(_dbConfiguration);

		var entity = schema.Tables.FirstOrDefault(s => s.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return entity is not null ? entity.Columns.Select(x => x.Name).ToDictionary(x => x) : throw new ArgumentException(string.Format(Errors.ENTITY_NOT_PRESENT_FORMAT, entityName), nameof(entityName));
	}

	public async Task<Dictionary<string, string>> GetEntityMappingsAsync( )
	{
		schema ??= await _schemaFetcher.GetSchemaAsync(_dbConfiguration);

		return schema.Tables.Select(x => x.Name).ToDictionary(x => x);
	}
}
