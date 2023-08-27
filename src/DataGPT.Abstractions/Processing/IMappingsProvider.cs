using DataGPT.Net.Abstractions.Types.Models;

namespace DataGPT.Net.Abstractions.Processing;
public interface IMappingsProvider
{
	Task<List<EntityMapping>> GetEntityMappingsAsync( );
}
