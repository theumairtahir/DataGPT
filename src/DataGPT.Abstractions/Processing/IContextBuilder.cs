using DataGPT.Net.Abstractions.Processing;

namespace DataGPT.Net.Abstractions.Processing;
public interface IContextBuilder
{
	void SetupContext(IMappingsProvider mappingsProvider, IRulesBuilder rulesBuilder);

	IQueryContext BuildContext( );
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
	public abstract void SetupContext(IMappingsProvider mappingsProvider, IRulesBuilder rulesBuilder);

	public virtual void SetupContext( ) => SetupContext(_mappingsProvider, _rulesBuilder);
}
