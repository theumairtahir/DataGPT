using DataGPT.Net.Abstractions.Params;
using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.Net.Abstractions.Infrastructure;
public interface IDataGptSetup
{
	IServiceCollection Services { get; }

	IDataGptSetup AddRule(Rule rule);

	List<Rule> Rules { get; }
}
