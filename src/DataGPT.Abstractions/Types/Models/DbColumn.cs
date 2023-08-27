namespace DataGPT.Net.Abstractions.Models;

public class DbColumn
{
	public required string Name { get; set; }
	public required string DataType { get; set; }

	public override bool Equals(object? obj) => obj is DbColumn other && other.Name == Name && other.DataType == DataType;

	public override int GetHashCode( ) => base.GetHashCode( );
}
