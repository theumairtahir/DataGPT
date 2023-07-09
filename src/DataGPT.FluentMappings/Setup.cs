using DataGPT.Abstractions.Processing;
using DataGPT.FluentMappings.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.FluentMappings;

public static class Setup
{
	public static void AddSimpleMappings(this IServiceCollection services) => services.AddScoped<IMappingsProvider, SimpleMappingsProvider>( );
}
