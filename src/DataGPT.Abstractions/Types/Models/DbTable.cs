namespace DataGPT.Abstractions.Types.Models;
public class DbTable
{
	public required string ObjectId { get; set; }
	public required string Name { get; set; }
	public bool IsView { get; set; }
}

public class DbColumn
{
	public required string Name { get; set; }
	public required string DataType { get; set; }
}
