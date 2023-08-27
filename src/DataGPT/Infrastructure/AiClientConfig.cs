namespace DataGPT.Net.Infrastructure;
public record AiClientConfig(string OrganizationId, string SecreteKey, int NumberOfRetries, int Variance);
