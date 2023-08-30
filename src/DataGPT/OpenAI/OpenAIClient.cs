using DataGPT.Net.Constants;
using DataGPT.Net.Exceptions;
using System.Net.Http.Json;

namespace DataGPT.Net.OpenAI;

internal interface IOpenAIClient
{
	Task<OpenAiCompletionResponse> PromptCompletionAsync(IOpenAiRequestBuilder builder);
}


internal class OpenAIClient : IOpenAIClient
{
	private readonly HttpClient _client;
	public OpenAIClient(IHttpClientFactory httpClientFactory)
	{
		_client = httpClientFactory.CreateClient(Strings.AI_CLIENT_NAME);
	}

	public async Task<OpenAiCompletionResponse> PromptCompletionAsync(IOpenAiRequestBuilder builder)
	{
		var request = builder.Build( );
		var response = await _client.PostAsJsonAsync("/v1/chat/completions", request);

		if (response is not HttpResponseMessage || !response.IsSuccessStatusCode || response.Content is null)
			throw new AiClientConnectionException(response.StatusCode);

		try
		{
			return await response.Content.ReadFromJsonAsync<OpenAiCompletionResponse>( ) ?? throw new AiResponseParsingException(await response.Content.ReadAsStringAsync( ));
		}
		catch (Exception ex)
		{
			throw new AiResponseParsingException(ex);
		}
	}
}
