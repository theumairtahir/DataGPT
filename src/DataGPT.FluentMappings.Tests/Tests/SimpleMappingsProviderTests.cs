using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Models;
using DataGPT.Net.Abstractions.Types.Models;
using DataGPT.Net.FluentMappings.Core;
using Moq;

namespace DataGPT.Net.FluentMappings.Tests.Tests;

[TestFixture]
internal class SimpleMappingsProviderTests
{
	[Test]
	public void Object_CreatesSuccessfully( )
	{
		//arrange
		var tables = new List<DbTable>
			{
				 new DbTable
				 {
					Name = "Students",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "StudentId",
						},
						new DbColumn
						{
							DataType = "Varchar",
							Name = "StudentName",
						},
						new DbColumn
						{
							DataType = "DateTime",
							Name = "DOB",
						},
					}
				 },
			};
		SimpleMappingsProvider simpleMappingsProvider;
		//act
		simpleMappingsProvider = new SimpleMappingsProvider(GetSchemaFetcher(tables));
		//assert
		Assert.That(simpleMappingsProvider, Is.Not.Null);
	}

	[TestCaseSource(nameof(GetTablesData))]
	public async Task GetEntityMappingsAsync_Returns_MappedListOfObjects(List<DbTable> tables)
	{
		//arrange
		var mappings = tables.Select(x => new EntityMapping { EntityName = x.Name, MappedTableName = x.Name, Attributes = x.Columns.Select(c => new AttributeMapping { AttributeName = c.Name, DbColumnName = c.Name, Type = c.DataType }).ToList( ) }).ToList( );
		//act
		var result = await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetEntityMappingsAsync( );
		//assert
		Assert.That(result, Is.EquivalentTo(mappings));
	}

	[Test]
	public async Task GetEntityMappingsAsync_ReturnsEmptyDictionary_WhenSchemaIsNull( )
	{
		//arrange
		var schemaFetcherMock = new Mock<ISchemaFetcher>( );
		schemaFetcherMock.Setup(x => x.GetSchemaAsync( )).ReturnsAsync(( ) => null!);
		var schemaFetcher = schemaFetcherMock.Object;
		//act
		var result = await new SimpleMappingsProvider(schemaFetcher).GetEntityMappingsAsync( );
		//assert
		Assert.Multiple(( ) =>
		{
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.Empty);
		});
	}

	[Test]
	public async Task GetEntityMappingsAsync_ReturnsEmptyDictionary_WhenEmptyColumns( )
	{
		//arrange
		var tables = new List<DbTable>( );
		//act
		var result = await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetEntityMappingsAsync( );
		//assert
		Assert.That(( ) => result, Is.Empty);
	}

	[Test]
	public async Task SubsequentCallDontTriggerAdditionalCallsTo_GetSchemaAsync( )
	{
		//arrange
		var schemaFetcherMock = new Mock<ISchemaFetcher>( );
		var counter = 0;
		schemaFetcherMock.Setup(x => x.GetSchemaAsync( )).Returns(async ( ) =>
		{
			counter++;
			return await Task.FromResult(new DbSchema( )
			{
				Tables = new List<DbTable>( )
				{
					 new DbTable
					 {
						  Name = "foo",
						   ObjectId="abc"
					 }
				}
			});
		});
		var schemaFetcher = schemaFetcherMock.Object;
		var simpleMappingsProvider = new SimpleMappingsProvider(schemaFetcher);
		//act
		for (int i = 0; i < 10; i++)
		{
			await simpleMappingsProvider.GetEntityMappingsAsync( );
		}
		//assert
		Assert.That(counter, Is.EqualTo(1));
	}


	private static ISchemaFetcher GetSchemaFetcher(List<DbTable> tables)
	{
		var schemaFetcherMock = new Mock<ISchemaFetcher>( );
		schemaFetcherMock.Setup(x => x.GetSchemaAsync( )).Returns(async ( ) => await Task.FromResult(new DbSchema( )
		{
			Tables = tables
		}));
		return schemaFetcherMock.Object;
	}

	private static List<DbTable>[ ] GetTablesData( )
	{
		return new List<DbTable>[ ]
		{
			new List<DbTable>
			{
				 new DbTable
				 {
					Name = "Students",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "StudentId",
						},
						new DbColumn
						{
							DataType = "Varchar",
							Name = "StudentName",
						},
						new DbColumn
						{
							DataType = "DateTime",
							Name = "DOB",
						},
					}
				 },
				new DbTable
				 {
					Name = "Courses",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "CourseId",
						},
						new DbColumn
						{
							DataType = "Varchar",
							Name = "CourseName",
						},
					}
				 },
				new DbTable
				 {
					Name = "StudentsCourses",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "PrimaryKey",
						},
						new DbColumn
						{
							DataType = "Integer",
							Name = "StudentId",
						},
						new DbColumn
						{
							DataType = "Integer",
							Name = "CourseId",
						},
					}
				 },
			},
			new List<DbTable>(),
			new List<DbTable>
			{
			   new DbTable
			   {
					Name = "Product",
					 ObjectId= Guid.NewGuid().ToString(),
			   },
			   new DbTable
			   {
					Name = "Category",
					ObjectId= Guid.NewGuid().ToString(),
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							 DataType = "Integer",
							  Name= "CategoryId",
						},
						new DbColumn
						{
							 DataType = "Varchar",
							  Name= "CategoryName",
						},
					}
			   }
			}
		};
	}
}
