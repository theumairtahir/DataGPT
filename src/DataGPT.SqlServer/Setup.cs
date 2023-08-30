using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Infrastructure;
using DataGPT.Net.SqlServer.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace DataGPT.Net.SqlServer;
public static class Setup
{
	public static IDataGptSetup AddSqlServer<T>(this IDataGptSetup gptSetup, T dbConfiguration) where T : IDbConfiguration, new()
	{
		gptSetup.Services.AddScoped<IDbConfiguration>(x => dbConfiguration);
		gptSetup.Services.AddTransient<IDbConnection>(x => new SqlConnection(dbConfiguration.ConnectionString));
		gptSetup.Services.AddDependencies( );
		return gptSetup;
	}

	public static IDataGptSetup AddSqlServer(this IDataGptSetup gptSetup, Func<IServiceProvider, IDbConfiguration> expression)
	{
		gptSetup.Services.AddSingleton(expression);
		gptSetup.Services.AddTransient<IDbConnection>(x => new SqlConnection(x.GetRequiredService<IDbConfiguration>( ).ConnectionString));
		gptSetup.Services.AddDependencies( );
		return gptSetup;
	}

	private static void AddDependencies(this IServiceCollection services)
	{
		services.AddSingleton<ISchemaFetcher, SqlSchemaFetcher>( );
		services.AddTransient<IDynamicQueryExecutor, SqlServerQueryExecutor>( );
	}
}
