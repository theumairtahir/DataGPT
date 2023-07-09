using DataGPT.Net.Abstractions.Processing;
using DataGPT.Net.FluentMappings.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.Net.FluentMappings;

public static class Setup
{
	public static void AddSimpleMappings(this IServiceCollection services) => services.AddScoped<IMappingsProvider, SimpleMappingsProvider>( );
}
