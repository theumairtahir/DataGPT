namespace DataGPT.Net.Abstractions.Processing;
public interface IContextBuilder
{
	IQueryContext BuildContext( );
	Task SetupContextAsync( );
}

public abstract class ContextBuilder : IContextBuilder
{
	private readonly IMappingsProvider _mappingsProvider;
	private readonly IRulesBuilder _rulesBuilder;

	protected ContextBuilder(IMappingsProvider mappingsProvider, IRulesBuilder rulesBuilder)
	{
		_mappingsProvider = mappingsProvider;
		_rulesBuilder = rulesBuilder;
	}

	public abstract IQueryContext BuildContext( );
	protected abstract Task SetupContextAsync(IMappingsProvider mappingsProvider, IRulesBuilder rulesBuilder);

	public virtual async Task SetupContextAsync( ) => await SetupContextAsync(_mappingsProvider, _rulesBuilder);
}
