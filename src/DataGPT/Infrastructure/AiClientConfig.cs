namespace DataGPT.Net.Infrastructure;
public record AiClientConfig(string OrganizationId, string SecreteKey, int NumberOfRetries = 1, double Variance = 0);
