namespace DataGPT.Net.Abstractions.Data;
public interface IDbConfiguration
{
	string ConnectionName { get; }
	string ConnectionString { get; }
	string DatabaseName { get; }
	string ServerType { get; }
}
