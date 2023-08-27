namespace DataGPT.Net.Abstractions.Models;
public class DbTable
{
	public required string ObjectId { get; set; }
	public required string Name { get; set; }
	public bool IsView { get; set; }
	public List<DbColumn> Columns { get; set; } = new( );

	public override bool Equals(object? obj) => obj is DbTable table && table.ObjectId == ObjectId && table.Name == Name && table.IsView == IsView && table.Columns.Count == Columns.Count && table.Columns.All(Columns.Contains);

	public override int GetHashCode( ) => base.GetHashCode( );
}
