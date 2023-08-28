using DataGPT.Net.Abstractions.Infrastructure;
using DataGPT.Net.Abstractions.Params;
using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.Net.Infrastructure;

internal class DataGptSetup : IDataGptSetup
{
	private readonly List<Rule> _rules = new( );

	public required IServiceCollection Services { get; set; }
	public List<Rule> Rules => _rules;

	public IDataGptSetup AddRule(Rule rule)
	{
		_rules.Add(rule);
		return this;
	}
}
