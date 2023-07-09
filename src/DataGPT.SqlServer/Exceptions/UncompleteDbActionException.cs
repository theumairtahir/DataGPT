﻿using DataGPT.Net.SqlServer.Constants;

namespace DataGPT.Net.SqlServer.Exceptions;

[Serializable]
public class UncompleteDbActionException : System.Exception
{
	public UncompleteDbActionException( ) : base(ErrorMessages.UNABLE_TO_COMPLETE_QUERY) { }
	public UncompleteDbActionException(string message) : base(message) { }
	public UncompleteDbActionException(string message, System.Exception inner) : base(message, inner) { }
	public UncompleteDbActionException(System.Exception inner) : base(ErrorMessages.UNABLE_TO_COMPLETE_QUERY, inner) { }

	protected UncompleteDbActionException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
}