using DataGPT.Abstractions.Params;

namespace DataGPT.Abstractions.Processing;
public interface IRulesBuilder
{
	IRulesBuilder AddRule(Rule rule);

	string BuildRules( );
}
