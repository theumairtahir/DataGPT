using DataGPT.Net.Abstractions.Params;
using DataGPT.Net.Abstractions.Processing;

namespace DataGPT.Net.Core;
internal class GptRulesBuilder : IRulesBuilder
{
	private readonly List<Rule> _rules;

	public GptRulesBuilder(List<Rule> rules)
	{
		_rules = rules;
	}

	public GptRulesBuilder( )
	{
		_rules = new List<Rule>( );
	}
	public IRulesBuilder AddRule(Rule rule)
	{
		_rules.Add(rule);
		return this;
	}

	public string BuildRules( ) => string.Join(",", _rules.Select(x => x.Instruction));
}
