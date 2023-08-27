namespace DataGPT.Net.Abstractions.Types.Models;
public class EntityMapping
{
	public required string EntityName { get; set; }
	public required string MappedTableName { get; set; }
	public List<AttributeMapping> Attributes { get; set; } = new( );

	public override bool Equals(object? obj) => obj is EntityMapping entityMapping && entityMapping.EntityName == EntityName && entityMapping.MappedTableName == MappedTableName && entityMapping.Attributes.Count == Attributes.Count && entityMapping.Attributes.All(Attributes.Contains);

	public override int GetHashCode( ) => base.GetHashCode( );

	public override string ToString( ) => $"{nameof(EntityName)}={EntityName}, {nameof(MappedTableName)}={MappedTableName}, {nameof(Attributes)}=[{string.Join('\n', Attributes)}]";
}

public class AttributeMapping
{
	public required string AttributeName { get; set; }
	public required string DbColumnName { get; set; }
	public required string Type { get; set; }

	public override bool Equals(object? obj) => obj is AttributeMapping attributeMapping && attributeMapping.AttributeName == AttributeName && attributeMapping.DbColumnName == DbColumnName && attributeMapping.Type == Type;

	public override int GetHashCode( ) => base.GetHashCode( );

	public override string ToString( ) => $"{nameof(AttributeName)}={AttributeName}, {nameof(DbColumnName)}={DbColumnName}, {nameof(Type)}={Type}";
}
