using DataGPT.Net.Abstractions.Params;

namespace DataGPT.Net.Abstractions.Processing;
public interface IRulesBuilder
{
	IRulesBuilder AddRule(Rule rule);

	string BuildRules( );
}
