using DataGPT.Net.Core;

namespace DataGPT.Net.Exceptions;


[Serializable]
public class UnableToBuildContextException : Exception
{
	private static readonly string EXCEPTION_MESSAGE = $"Unable to build context. Make sure to call {nameof(GptContextBuilder.SetupContextAsync)} before building the context.";

	public UnableToBuildContextException( ) : base(EXCEPTION_MESSAGE) { }

	public UnableToBuildContextException(Exception inner) : base(EXCEPTION_MESSAGE, inner) { }
	protected UnableToBuildContextException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
