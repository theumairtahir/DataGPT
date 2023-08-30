namespace DataGPT.Net.Exceptions;

[Serializable]
public class InvalidAiResponseException : Exception
{
	private const string ERROR_MESSAGE = "AI service have got invalid response to the query.";
	public InvalidAiResponseException(string aiResponse) : base(ERROR_MESSAGE + $" Response: {aiResponse}") { }
	protected InvalidAiResponseException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
