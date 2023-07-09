namespace DataGPT.Net.Abstractions.Processing;
public interface IMappingsProvider
{
	Task<Dictionary<string, string>> GetEntityMappingsAsync( );
	Task<Dictionary<string, string>> GetColumnMappingsAsync(string entityName);
}
