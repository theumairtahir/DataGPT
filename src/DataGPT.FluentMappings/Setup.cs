using DataGPT.Net.Abstractions.Infrastructure;
using DataGPT.Net.Abstractions.Processing;
using DataGPT.Net.FluentMappings.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.Net.FluentMappings;

public static class Setup
{
	public static IDataGptSetup AddSimpleMappings(this IDataGptSetup gptSetup)
	{
		gptSetup.Services.AddSingleton<IMappingsProvider, SimpleMappingsProvider>( );
		return gptSetup;
	}
}
