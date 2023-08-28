using DataGPT.Net.Abstractions.Infrastructure;
using DataGPT.Net.Abstractions.Processing;
using DataGPT.Net.Constants;
using DataGPT.Net.Core;
using DataGPT.Net.Infrastructure;
using DataGPT.Net.OpenAI;
using Microsoft.Extensions.DependencyInjection;

namespace DataGPT.Net;
public static class Setup
{
	public static IDataGptSetup AddDataGpt(this IServiceCollection services, AiClientConfig config)
	{
		services.AddSingleton(s => config);
		return services.AddDataGpt( );
	}


	public static IDataGptSetup AddDataGpt(this IServiceCollection services, Func<IServiceProvider, AiClientConfig> aiClientConfigFunction)
	{
		services.AddSingleton(s => aiClientConfigFunction(s));
		return services.AddDataGpt( );
	}

	public static void Build(this IDataGptSetup setup) => setup.Services.AddSingleton<IRulesBuilder>(sp => new GptRulesBuilder(setup.Rules));

	private static IDataGptSetup AddDataGpt(this IServiceCollection services)
	{
		//Singleton Services
		services.AddSingleton<IContextBuilder, GptContextBuilder>( );

		//Scoped Services
		services.AddScoped<IOpenAIClient, OpenAIClient>( );

		//Transient Services
		services.AddTransient<IQueryProcessingService, QueryProcessingService>( );


		services.AddHttpClient<HttpClient>(Strings.AI_CLIENT_NAME, (sp, client) =>
		{
			var aiClientConfig = sp.GetRequiredService<AiClientConfig>( );
			client.BaseAddress = new Uri("https://api.openai.com/");
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {aiClientConfig.SecreteKey}");
		});

		return new DataGptSetup { Services = services };
	}
}
