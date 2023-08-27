using DataGPT.Net.Abstractions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.Net.Infrastructure;

internal class DataGptSetup : IDataGptSetup
{
	public required IServiceCollection Services { get; set; }
}
