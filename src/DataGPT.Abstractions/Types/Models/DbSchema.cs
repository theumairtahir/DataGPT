namespace DataGPT.Abstractions.Types.Models;

public class DbSchema
{
	public List<DbTable> Tables { get; set; } = new( );
}
