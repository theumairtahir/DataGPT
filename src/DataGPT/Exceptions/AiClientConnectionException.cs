using System.Net;

namespace DataGPT.Net.Exceptions;

[Serializable]
public class AiClientConnectionException : Exception
{
	private const string ERROR_MESSAGE = "Unable to connect to the AI Servers.";

	public AiClientConnectionException(HttpStatusCode responseCode) : base(ERROR_MESSAGE + $" Response Code: {responseCode}") { }
	protected AiClientConnectionException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
