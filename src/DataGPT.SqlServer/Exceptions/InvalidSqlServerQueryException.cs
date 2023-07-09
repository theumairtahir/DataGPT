using DataGPT.Net.SqlServer.Constants;

namespace DataGPT.Net.SqlServer.Exceptions;


[Serializable]
public class InvalidSqlServerQueryException : Exception
{
	public InvalidSqlServerQueryException(string providedQuery) : base($"{ErrorMessages.INVALID_SQL_QUERY} Query: {providedQuery}") { }
	protected InvalidSqlServerQueryException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
