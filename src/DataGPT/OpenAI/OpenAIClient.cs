using DataGPT.Net.Constants;
using System.Net.Http.Json;

namespace DataGPT.Net.OpenAI;

internal interface IOpenAIClient
{
	Task<OpenAiCompletionResponse?> PromptCompletionAsync(IOpenAiRequestBuilder builder);
}


internal class OpenAIClient : IOpenAIClient
{
	private readonly HttpClient _client;
	public OpenAIClient(IHttpClientFactory httpClientFactory)
	{
		_client = httpClientFactory.CreateClient(Strings.AI_CLIENT_NAME);
	}

	public async Task<OpenAiCompletionResponse?> PromptCompletionAsync(IOpenAiRequestBuilder builder)
	{
		try
		{
			var request = builder.Build( );
			var response = await _client.PostAsJsonAsync("/chat/completions", request);
			if (response is HttpResponseMessage && response.IsSuccessStatusCode && response.Content is not null)
				return await response.Content.ReadFromJsonAsync<OpenAiCompletionResponse>( );
		}
		catch (Exception)
		{

		}
		return null;
	}
}
