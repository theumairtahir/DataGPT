using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Processing;
using DataGPT.Net.Exceptions;
using System.Text;

namespace DataGPT.Net.Core;
internal class GptContextBuilder : ContextBuilder
{
	private readonly IDbConfiguration _dbConfiguration;
	private string? systemContext;
	public GptContextBuilder(IMappingsProvider mappingsProvider, IRulesBuilder rulesBuilder, IDbConfiguration dbConfiguration) : base(mappingsProvider, rulesBuilder)
	{
		_dbConfiguration = dbConfiguration;
	}

	public override IQueryContext BuildContext( ) => systemContext is not null ? new QueryContext(systemContext) : throw new UnableToBuildContextException( );

	protected override async Task SetupContextAsync(IMappingsProvider mappingsProvider, IRulesBuilder rulesBuilder)
	{
		if (systemContext is not null)
			return;

		var stringBuilder = new StringBuilder( ).AppendLine($"You are a {_dbConfiguration.ServerType} Query generation system whose job is to generate SQL Queries for user on demand by converting his natural language according to the a given database structure and the instructions given along with the query. Please generate code only we don't need any type of other text.");

		var entityMappings = await mappingsProvider.GetEntityMappingsAsync( );

		if (entityMappings is null || entityMappings.Count == 0) return;

		stringBuilder.AppendLine("Given a SQL db with the following tables:");
		foreach (var entity in entityMappings)
		{
			stringBuilder.Append($"Entity Name: ")
				.Append(entity.EntityName);

			if (string.Compare(entity.EntityName, entity.MappedTableName, StringComparison.OrdinalIgnoreCase) is not 0)
				stringBuilder.Append($", which is mapped to database table named '{entity.MappedTableName}',");

			stringBuilder.Append(" has the given attributes i.e. ");
			stringBuilder.AppendJoin(',', entity.Attributes.Select(attr =>
			{
				var columnBuilder = new StringBuilder(attr.AttributeName);

				if (string.Compare(attr.AttributeName, attr.DbColumnName, StringComparison.OrdinalIgnoreCase) is not 0)
					columnBuilder.Append($" is mapped to the table's column named '{attr.DbColumnName}'");

				return columnBuilder.Append($" with a datatype of {attr.Type}").ToString( );
			}));

			stringBuilder.AppendLine( );
		}

		stringBuilder.AppendLine("Instructions:");
		stringBuilder.AppendLine("- Please use square brackets ([TableName or columnName]) if there's any entity name conflicts with the sql keyword or reserve word");
		stringBuilder.AppendLine("- Make sure that there is no column without a proper name");
		stringBuilder.AppendLine("- Always assign alias with the table names and use that alias in the select statement");
		stringBuilder.AppendLine($"- Always send a complete query with valid syntax according to the {_dbConfiguration.ServerType}");
		stringBuilder.AppendLine("- If there is a foreign key in a table / result then join it with the referenced table and get a minimal details");
		stringBuilder.AppendLine("- Be smart don't use the Tables/Columns which are not provided with the entities details");
		stringBuilder.AppendLine($"- {rulesBuilder.BuildRules( )}");

		systemContext = stringBuilder.ToString( );
	}

	private record QueryContext(string ContextHeader) : IQueryContext;
}
