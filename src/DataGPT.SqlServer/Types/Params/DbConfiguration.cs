using DataGPT.Net.Abstractions.Data;

namespace DataGPT.Net.SqlServer.Types.Params;
public record DbConfiguration(string ConnectionName, string ConnectionString, string DatabaseName, string ServerType, int ConnectionTimeout) : IDbConfiguration;
