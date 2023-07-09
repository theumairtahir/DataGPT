using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.SqlServer.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace DataGPT.Net.SqlServer;
public static class Setup
{
	public static void AddSqlServer<T>(this IServiceCollection services, T dbConfiguration) where T : IDbConfiguration, new()
	{
		services.AddScoped<IDbConfiguration>(x => dbConfiguration);
		services.AddTransient<IDbConnection>(x => new SqlConnection(dbConfiguration.ConnectionString));
		services.AddDependencies( );
	}

	public static void AddSqlServer(this IServiceCollection services)
	{
		services.AddTransient<IDbConnection>(x => new SqlConnection(x.GetRequiredService<IDbConfiguration>( ).ConnectionString));
		services.AddDependencies( );
	}

	private static void AddDependencies(this IServiceCollection services)
	{
		services.AddScoped<ISchemaFetcher, SqlSchemaFetcher>( );
	}
}
