using DataGPT.Net.SqlServer.Constants;

namespace DataGPT.Net.SqlServer.Exceptions;

[Serializable]
public class IncompleteDbActionException : System.Exception
{
	public IncompleteDbActionException( ) : base(ErrorMessages.UNABLE_TO_COMPLETE_QUERY) { }
	public IncompleteDbActionException(string message) : base(message) { }
	public IncompleteDbActionException(string message, System.Exception inner) : base(message, inner) { }
	public IncompleteDbActionException(System.Exception inner) : base(ErrorMessages.UNABLE_TO_COMPLETE_QUERY, inner) { }

	protected IncompleteDbActionException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
}
