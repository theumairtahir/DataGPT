using DataGPT.Net.Constants;
using System.Text.Json.Serialization;

namespace DataGPT.Net.OpenAI;


internal record OpenAiRequest(string Model, double Temperature, List<OpenAiMessage> Messages);

internal record OpenAiMessage(string Role, string Content);

internal record OpenAiCompletionResponse(string Id, string Object, long Created, string Model, OpenAiUsage Usage, List<OpenAiChoice> Choices);

internal record OpenAiChoice(int Index, OpenAiMessage Message)
{
	[JsonPropertyName(Strings.PROP_FINISH_REASON)]
	public string FinishReason { get; set; } = null!;
}

internal record OpenAiUsage( )
{
	[JsonPropertyName(Strings.PROP_PROMPT_TOKENS)]
	public int PromptTokens { get; set; }

	[JsonPropertyName(Strings.PROP_COMPLETION_TOKENS)]
	public int CompletionTokens { get; set; }

	[JsonPropertyName(Strings.PROP_TOTAL_TOKENS)]
	public int TotalTokens { get; set; }
}
