using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Processing;
using DataGPT.Net.Infrastructure;
using DataGPT.Net.OpenAI;

namespace DataGPT.Net.Core;

public interface IQueryProcessingService
{
	Task<IEnumerable<dynamic>> ProcessAsync(string naturalQuery);
}

internal class QueryProcessingService : IQueryProcessingService
{
	private const string QUERY_SELECT = "SELECT";
	private readonly IOpenAIClient _aiClient;
	private readonly IQueryContext _context;
	private readonly IDynamicQueryExecutor _queryExecutor;
	private readonly AiClientConfig _aiConfig;

	public QueryProcessingService(IOpenAIClient aiClient, IQueryContext context, IDynamicQueryExecutor queryExecutor, AiClientConfig aiConfig)
	{
		_aiClient = aiClient;
		_context = context;
		_queryExecutor = queryExecutor;
		_aiConfig = aiConfig;
	}


	public async Task<IEnumerable<dynamic>> ProcessAsync(string naturalQuery)
	{
		var aiClientResponse = await _aiClient.PromptCompletionAsync(new NaturalQueryProcessingRequestBuilder(OpenAiModels.GPT_3_5_TURBO, _aiConfig.Variance, _context.ContextHeader, naturalQuery)) ?? throw new Exception( );

		var sqlQuery = aiClientResponse.Choices.FirstOrDefault( )?.Message.Content;

		if (sqlQuery is null || !sqlQuery.Contains(QUERY_SELECT, StringComparison.OrdinalIgnoreCase))
			throw new Exception( );

		var startingPointOfSelect = sqlQuery.IndexOf(QUERY_SELECT, StringComparison.OrdinalIgnoreCase);
		if (startingPointOfSelect is not 0)
			sqlQuery = sqlQuery.Remove(0, startingPointOfSelect);

		return await _queryExecutor.ExecuteQueryAsync(sqlQuery);
	}


	private class NaturalQueryProcessingRequestBuilder : IOpenAiRequestBuilder
	{
		private const string ROLE_SYSTEM = "system";
		private const string ROLE_USER = "user";
		private readonly string _model;
		private readonly double _variance;
		private readonly string _context;
		private readonly string _query;

		public NaturalQueryProcessingRequestBuilder(string model, double variance, string context, string query)
		{
			_model = model;
			_variance = variance;
			_context = context;
			_query = query;
		}

		public OpenAiRequest Build( )
		{
			var messages = new List<OpenAiMessage>( )
			{
				new OpenAiMessage(ROLE_SYSTEM, _context),
				new OpenAiMessage(ROLE_USER, _query)
			};

			return new OpenAiRequest(_model, _variance, messages);
		}
	}
}
