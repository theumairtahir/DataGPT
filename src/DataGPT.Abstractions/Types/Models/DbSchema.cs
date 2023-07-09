namespace DataGPT.Net.Abstractions.Models;

public class DbSchema
{
	public List<DbTable> Tables { get; set; } = new( );
}
