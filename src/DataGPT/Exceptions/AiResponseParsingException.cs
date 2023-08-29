namespace DataGPT.Net.Exceptions;

[Serializable]
public class AiResponseParsingException : Exception
{
	public const string ERROR_MESSAGE = "Unable to parse the response from AI Service.";

	public AiResponseParsingException(string responseBody) : base(ERROR_MESSAGE + $" Response Body: {responseBody}") { }
	public AiResponseParsingException(Exception inner) : base(ERROR_MESSAGE, inner) { }
	protected AiResponseParsingException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
