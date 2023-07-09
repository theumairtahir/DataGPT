namespace DataGPT.Net.SqlServer.Types.Models;
internal class SqlResultColumn
{
	public int ObjectId { get; set; }
	public required string TableName { get; set; }
	public required string ColumnName { get; set; }
	public bool IsView { get; set; }
	public required string DataType { get; set; }
}
