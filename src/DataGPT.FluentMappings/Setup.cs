using DataGPT.Abstractions.Infrastructure;

namespace DataGPT.FluentMappings;

public static class Setup
{
	public static IDataGptInjectable AddFluentMappings(this IDataGptInjectable injectable)
	{
		return injectable;
	}
}
