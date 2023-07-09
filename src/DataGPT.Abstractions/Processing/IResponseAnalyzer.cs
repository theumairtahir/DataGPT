using DataGPT.Net.Abstractions.Params;

namespace DataGPT.Net.Abstractions.Processing;
public interface IResponseAnalyzer
{
	string GetDBQuery( );
	Dictionary<string, object> GetQueryParameters( );
	List<Instruction> GetInstructions( );
}

public abstract class AbstractResponseAnalyzer : IResponseAnalyzer
{
	protected readonly AiResponse _response;

	protected AbstractResponseAnalyzer(AiResponse response)
	{
		_response = response;
	}
	public abstract string GetDBQuery( );
	public abstract List<Instruction> GetInstructions( );
	public abstract Dictionary<string, object> GetQueryParameters( );
}
