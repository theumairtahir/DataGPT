using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Types.Models;

namespace DataGPT.Net.FluentMappings.Core;

internal class SimpleMappingsProvider : AbstractMappingsProvider
{
	public SimpleMappingsProvider(ISchemaFetcher schemaFetcher) : base(schemaFetcher)
	{
	}

	public override async Task<List<EntityMapping>> GetEntityMappingsAsync( )
	{
		var schema = await GetSchemaAsync( );

		return schema is not null ? schema.Tables.Select(x => new EntityMapping { EntityName = x.Name, MappedTableName = x.Name, Attributes = x.Columns.Select(c => new AttributeMapping { AttributeName = c.Name, DbColumnName = c.Name, Type = c.DataType }).ToList( ) }).ToList( ) : new List<EntityMapping>( );
	}
}
