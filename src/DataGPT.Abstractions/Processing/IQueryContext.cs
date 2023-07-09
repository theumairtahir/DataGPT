namespace DataGPT.Net.Abstractions.Processing;

public interface IQueryContext
{
	string ContextHeader { get; }
	string DefinedRules { get; }

	string GetCompiledTokens( );
}
