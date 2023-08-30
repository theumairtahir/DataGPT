using DataGPT.Net;
using DataGPT.Net.FluentMappings;
using DataGPT.Net.Infrastructure;
using DataGPT.Net.RestApi.Extensions;
using DataGPT.Net.SqlServer;
using DataGPT.Net.SqlServer.Types.Params;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer( );
builder.Services.AddSwaggerGen( );
builder.Services.AddDataGpt(sp =>
{
	var openAiSecrete = sp.GetRequiredService<IConfiguration>( ).GetValue<string>("OpenAiSecrete");
	return new AiClientConfig("", openAiSecrete!, 3, 0.7);
}).AddSqlServer(sp =>
{
	var conString = sp.GetRequiredService<IConfiguration>( ).GetConnectionString("Digisuite");
	return new DbConfiguration("Default", conString!, "DemoDatabase", "MS SQL Server", 300);
}).AddSimpleMappings( ).Build( );

var app = builder.Build( );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment( ))
{
	app.UseSwagger( );
	app.UseSwaggerUI( );
}

app.UseHttpsRedirection( );

app.MapDataGpt( ).WithOpenApi( );


var summaries = new[ ]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", ( ) =>
{
	var forecast = Enumerable.Range(1, 5).Select(index =>
		new WeatherForecast
		(
			DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			Random.Shared.Next(-20, 55),
			summaries[Random.Shared.Next(summaries.Length)]
		))
		.ToArray( );
	return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi( );

app.Run( );

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)( TemperatureC / 0.5556 );
}
