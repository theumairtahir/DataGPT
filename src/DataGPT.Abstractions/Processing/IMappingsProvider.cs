namespace DataGPT.Abstractions.Processing;
public interface IMappingsProvider
{
	Dictionary<string, string> GetEntityMappings( );
	Dictionary<string, string> GetColumnMappings(string entityName);
}
