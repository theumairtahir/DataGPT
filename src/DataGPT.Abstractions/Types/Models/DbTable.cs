namespace DataGPT.Net.Abstractions.Models;
public class DbTable
{
	public required string ObjectId { get; set; }
	public required string Name { get; set; }
	public bool IsView { get; set; }
	public List<DbColumn> Columns { get; set; } = new( );
}
