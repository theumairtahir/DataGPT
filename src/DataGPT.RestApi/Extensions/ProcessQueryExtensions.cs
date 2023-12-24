using DataGPT.Net.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DataGPT.Net.RestApi.Extensions;

public static class ProcessQueryExtensions
{
	public static RouteHandlerBuilder MapDataGpt(this IEndpointRouteBuilder appBuilder)
	{
		return appBuilder.MapGet("/datagpt", async ([FromQuery] string query, IQueryProcessingService queryService) => await queryService.ProcessAsync(query)).WithName("DataGPT");
	}
}
