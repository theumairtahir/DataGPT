using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.Abstractions.Infrastructure;
public interface IDataGptInjectable
{
	IServiceCollection ServiceCollection { get; }

	IServiceProvider ServiceProvider { get; }
}
