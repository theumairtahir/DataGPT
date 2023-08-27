using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.Net.Abstractions.Infrastructure;
public interface IDataGptSetup
{
	IServiceCollection Services { get; }
}
