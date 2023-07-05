using DataGPT.Abstractions.Infrastructure;
using DataGPT.Abstractions.Processing;
using DataGPT.FluentMappings.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.FluentMappings;

public static class Setup
{
	public static IDataGptInjectable AddSimpleMappings(this IDataGptInjectable injectable)
	{
		injectable.ServiceCollection.AddScoped<IMappingsProvider, SimpleMappingsProvider>( );
		return injectable;
	}
}
