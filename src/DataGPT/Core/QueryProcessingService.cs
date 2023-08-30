using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Processing;
using DataGPT.Net.Exceptions;
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
	private const string STRING_NULL = "[null]";
	private readonly IOpenAIClient _aiClient;
	private readonly IContextBuilder _contextBuilder;
	private readonly IDynamicQueryExecutor _queryExecutor;
	private readonly AiClientConfig _aiConfig;

	public QueryProcessingService(IOpenAIClient aiClient, IContextBuilder contextBuilder, IDynamicQueryExecutor queryExecutor, AiClientConfig aiConfig)
	{
		_aiClient = aiClient;
		_contextBuilder = contextBuilder;
		_queryExecutor = queryExecutor;
		_aiConfig = aiConfig;
	}

	public async Task<IEnumerable<dynamic>> ProcessAsync(string naturalQuery)
	{
		var retries = 0;
		Exception queryExecutorException;
		await _contextBuilder.SetupContextAsync( );
		var context = _contextBuilder.BuildContext( );
		var gptModel = _contextBuilder.TokensCount( ) switch
		{
			> 15000 => OpenAiModels.GPT_4_32K,
			> 3000 and <= 15000 => OpenAiModels.GPT_3_5_TURBO_16K,
			_ => OpenAiModels.GPT_3_5_TURBO
		};
		var naturalQueryBuilder = new NaturalQueryProcessingRequestBuilder(gptModel, _aiConfig.Variance, context.ContextHeader, naturalQuery).CreateRequest( );
		do
		{
			var aiClientResponse = await _aiClient.PromptCompletionAsync(naturalQueryBuilder);

			var sqlQuery = aiClientResponse.Choices.FirstOrDefault( )?.Message.Content;

			if (sqlQuery is null || !sqlQuery.Contains(QUERY_SELECT, StringComparison.OrdinalIgnoreCase))
				throw new InvalidAiResponseException(sqlQuery ?? STRING_NULL);

			var startingPointOfSelect = sqlQuery.IndexOf(QUERY_SELECT, StringComparison.OrdinalIgnoreCase);
			if (startingPointOfSelect is not 0)
				sqlQuery = sqlQuery.Remove(0, startingPointOfSelect);

			try
			{
				return await _queryExecutor.ExecuteQueryAsync(sqlQuery);
			}
			catch (Exception ex)
			{
				queryExecutorException = ex;

				if (retries <= _aiConfig.NumberOfRetries && queryExecutorException.InnerException is not null && !string.IsNullOrEmpty(queryExecutorException.InnerException.Message))
				{
					naturalQueryBuilder.AddExceptionMessage(aiClientResponse, $"We have got an exception in the last execution which states the message '{queryExecutorException.InnerException.Message}'. Please try to resolve the error in the query and send a new response");
				}
			}
			retries++;
		} while (retries <= _aiConfig.NumberOfRetries);
		throw queryExecutorException;
	}


	private class NaturalQueryProcessingRequestBuilder : IOpenAiRequestBuilder
	{
		private const string ROLE_SYSTEM = "system";
		private const string ROLE_USER = "user";
		private readonly string _model;
		private readonly double _variance;
		private readonly string _context;
		private readonly string _query;
		private readonly List<OpenAiMessage> _messages = new( );

		public NaturalQueryProcessingRequestBuilder(string model, double variance, string context, string query)
		{
			_model = model;
			_variance = variance;
			_context = context;
			_query = query;
		}

		public NaturalQueryProcessingRequestBuilder CreateRequest( )
		{
			_messages.Add(new OpenAiMessage(ROLE_SYSTEM, _context));
			_messages.Add(new OpenAiMessage(ROLE_USER, _query));
			return this;
		}

		public OpenAiRequest Build( ) => new(_model, _variance, _messages);

		public NaturalQueryProcessingRequestBuilder AddExceptionMessage(OpenAiCompletionResponse lastResponse, string message)
		{
			_messages.Add(lastResponse.Choices.First( ).Message);
			_messages.Add(new OpenAiMessage(ROLE_USER, message));
			return this;
		}
	}
}
