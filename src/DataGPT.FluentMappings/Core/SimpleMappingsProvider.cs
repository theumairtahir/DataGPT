using DataGPT.Net.Abstractions.Data;

namespace DataGPT.Net.FluentMappings.Core;

internal class SimpleMappingsProvider : AbstractMappingsProvider
{
	public SimpleMappingsProvider(ISchemaFetcher schemaFetcher) : base(schemaFetcher)
	{
	}

	public override async Task<Dictionary<string, string>> GetColumnMappingsAsync(string entityName)
	{
		if (entityName == null)
			throw new ArgumentNullException(nameof(entityName));

		var schema = await GetSchemaAsync( );

		var entity = schema.Tables.FirstOrDefault(s => s.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return entity is not null ? entity.Columns.Select(x => x.Name).ToDictionary(x => x) : throw EntityNotPresentException(entityName, nameof(entityName));
	}

	public override async Task<Dictionary<string, string>> GetEntityMappingsAsync( )
	{
		var schema = await GetSchemaAsync( );

		return schema.Tables.Select(x => x.Name).ToDictionary(x => x);
	}
}
